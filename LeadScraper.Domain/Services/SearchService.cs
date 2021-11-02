﻿using LeadScraper.Domain.Contracts;
using LeadScraper.Domain.Models;
using LeadScraper.Domain.Models.Requests;
using LeadScraper.Domain.Models.Responses;
using LeadScraper.Infrastructure.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LeadScraper.Domain.Services
{
    public class SearchService : ISearchService
    {
        private readonly ISettingsService _settingsService;
        private readonly IWhoIsServerService _whoIsServerService;
        private readonly IHttpClientFactory _httpClientFactory;
        const string endpoint = "https://api.bing.microsoft.com/v7.0/search";
        public SearchService(ISettingsService settingsService, IWhoIsServerService whoIsServerService, IHttpClientFactory httpClientFactory)
        {
            _settingsService = settingsService;
            _whoIsServerService = whoIsServerService;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<List<LeadItem>> Search(SearchRequest request)
        {
            var settings = await _settingsService.GetAsync();
            SearchResult result = await GetBingSearchResult(request, settings);
            List<WhoIsServerResponse> whoIsServers = _whoIsServerService.GetAll();
            List<LeadItem> leads = GetLeadItems(settings, result, whoIsServers);
            List<LeadItem> finalLeads = GetPhoneNumber(leads);
            return await Task.Run(()=> finalLeads);

        }


        private async Task<SearchResult> GetBingSearchResult(SearchRequest request, SettingResponse settings)
        {
            string uriQuery = ConstructSearchUri(request);
            string json = await GetSearchResultJson(uriQuery, settings);
            dynamic pardjson = JsonConvert.DeserializeObject(json);
            SearchResult result = JsonConvert.DeserializeObject<SearchResult>(json);

            return result;
        }

        private string ConstructSearchUri(SearchRequest request)
        {
            var uriQuery = endpoint + "?q=" + Uri.EscapeDataString(request.SearchTerm);
            uriQuery = uriQuery + "&count=" + request.ResultsPerPage;
            uriQuery = uriQuery + "&offset=" + ((request.StartingPage - 1) * request.ResultsPerPage);

            if (request.CountryCode == "All" || request.CountryCode == null) return uriQuery;

            uriQuery = uriQuery + "&cc=" + request.CountryCode;

            return uriQuery;
        }

        private async Task<string> GetSearchResultJson(string uri, SettingResponse settings)
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
            uri);
            request.Headers.Add("Ocp-Apim-Subscription-Key", settings.BingKey);

            var client = _httpClientFactory.CreateClient();
            var response = await client.SendAsync(request);

            string json = new StreamReader(response.Content.ReadAsStream()).ReadToEnd();

            return json;
        }

        private bool UriContainsBlackListTerm(SettingResponse settings, WebPagesValue webPage)
        {
            List<string> blackListTermsList = new List<string>(settings.BlackListTerms.Split(','));
            return blackListTermsList.Any(s => webPage.Url.AbsoluteUri.Contains(s));
        }

        private bool UriContainsWhiteListTld(SettingResponse settings, WebPagesValue webPage)
        {
            List<string> whiteListTldList = new List<string>(settings.WhiteListTlds.Split(','));
            return whiteListTldList.Any(s => webPage.Url.AbsoluteUri.Contains(s));
        }

        private bool UriEndsWithWhiteListTld(SettingResponse settings, WebPagesValue webPage)
        {
            List<string> whiteListTldList = new List<string>(settings.WhiteListTlds.Split(','));
            return whiteListTldList.Any(s => webPage.Url.AbsoluteUri.Contains(s));
        }

        private string CleanUri(SettingResponse settings, string uri, WebPagesValue webPage)
        {
            if (uri.IndexOf('?') > -1)
            {
                uri = uri.Substring(0, uri.IndexOf('?'));
            }

            if (UriEndsWithWhiteListTld(settings, webPage)) return uri;

            uri = uri.Replace(webPage.Url.AbsolutePath, "");
            uri = uri.Replace("!", "");
            uri = uri.Replace("#", "");
            return CleanUri(settings, uri, webPage);

        }

        private string FindContactUrl(WebPagesValue webPage)
        {
            var contact = webPage.DeepLinks?.FirstOrDefault(l => l.Name.Contains("Contact"))?.Url?.ToString();
            if (contact != null) return contact;
            return webPage.DeepLinks?.FirstOrDefault(l => l.Name.Contains("About"))?.Url?.ToString();
        }

        private List<LeadItem> GetLeadItems(SettingResponse settings, SearchResult result, List<WhoIsServerResponse> whoIsServers)
        {
            List<LeadItem> leads = new List<LeadItem>();

            foreach (var webPage in result.WebPages.Value)
            {
                if (!UriContainsBlackListTerm(settings, webPage) && UriContainsWhiteListTld(settings, webPage))
                {
                    //foreach(var tld in settings.WhiteListTlds.Split(','))
                    //{
                    //    var server = whoIsServers.FirstOrDefault(l => l.Tld == tld);
                    //    if (server == null) break;
                    //    var whoIsInfo = GetWhoisInformation(server.Server, webPage.Url.Host);

                    //}
                    LeadItem lead = new LeadItem();
                    lead.Name = webPage.Name;
                    lead.Url = webPage.Url.ToString();
                    lead.Host = webPage.Url.Host;
                    lead.ContactUrl = FindContactUrl(webPage);
                    leads.Add(lead);

                }

            }

            return leads = leads.GroupBy(elem => elem.Host).Select(group => group.First()).ToList();
        }

        private List<LeadItem> GetPhoneNumber(List<LeadItem> leads)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.102 Safari/537.36 Edge/18.18363");
            List<LeadItem> finalList = new List<LeadItem>();
            Regex phoneNumExp = new Regex(@"(\({0,1}\d{3}\){0,1}[- \.]\d{3}[- \.]\d{4})|(\+\d{2}-\d{2,4}-\d{3,4}-\d{3,4})");
            foreach (var lead in leads)
            {
                string html = "";
                try
                {
                    var url = lead.ContactUrl ?? lead.Url;
                    html = httpClient.GetStringAsync(url).Result;
                    int phoneIndex = html.IndexOf("phone");
                    if (phoneIndex == -1)
                    {
                        phoneIndex = html.IndexOf("tel");
                    }
                    if (phoneIndex == -1) throw new ArgumentException("no phone number on page");
                    var shortHtml = html.Substring(phoneIndex);

                    var match = phoneNumExp.Match(shortHtml);

                    if (!match.Success) throw new ArgumentException("no phone number on page");
                    lead.Phone = match.Value;

                }
                catch (Exception ex)
                {
                    string number = "COULDNT SCRAPE";
                    lead.Phone = number;

                }
                finalList.Add(lead);

            }
            return finalList;
        }
        private string GetWhoisInformation(string whoisServer, string url)
        {
            StringBuilder stringBuilderResult = new StringBuilder();
            TcpClient tcpClinetWhois = new TcpClient(whoisServer, 43);
            NetworkStream networkStreamWhois = tcpClinetWhois.GetStream();
            BufferedStream bufferedStreamWhois = new BufferedStream(networkStreamWhois);
            StreamWriter streamWriter = new StreamWriter(bufferedStreamWhois);

            streamWriter.WriteLine(url);
            streamWriter.Flush();

            StreamReader streamReaderReceive = new StreamReader(bufferedStreamWhois);

            while (!streamReaderReceive.EndOfStream)
                stringBuilderResult.AppendLine(streamReaderReceive.ReadLine());

            return stringBuilderResult.ToString();
        }
    }
}
