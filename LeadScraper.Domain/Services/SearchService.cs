using LeadScraper.Domain.Contracts;
using LeadScraper.Domain.Models;
using LeadScraper.Domain.Models.Requests;
using LeadScraper.Domain.Models.Responses;
using LeadScraper.Infrastructure.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
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
        private readonly IHttpClientFactory _httpClientFactory;
        const string endpoint = "https://api.bing.microsoft.com/v7.0/search";
        private List<string> _blackListTermsList = new List<string>();
        private List<string> _whiteListTldList = new List<string>();
        public SearchService(ISettingsService settingsService, IHttpClientFactory httpClientFactory)
        {
            _settingsService = settingsService;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<HashSet<LeadItem>> Search(SearchRequest request)
        {
            var settings = await _settingsService.GetAsync();
            _blackListTermsList = new List<string>(settings.BlackListTerms.Split(','));
            _whiteListTldList = new List<string>(settings.WhiteListTlds.Split(','));
            HashSet<SearchResult> results = new HashSet<SearchResult>();
            int toSubtract = 0;
            for (int i = 0; i < request.Pages; i++)
            {
                SearchResult result = await GetBingSearchResult(request, settings);
                results.Add(result);
                request.StartingPage++;
                toSubtract++;
            }
            request.StartingPage -= toSubtract;
            HashSet<LeadItem> leads = GetLeadItems(settings, results);
            HashSet<LeadItem> finalLeads = await GetPhoneNumber(leads);
            return await Task.Run(()=> finalLeads);
        }

        private async Task<SearchResult> GetBingSearchResult(SearchRequest request, SettingResponse settings)
        {
            string uriQuery = ConstructSearchUri(request);
            string json = await GetSearchResultJson(uriQuery, settings);
            SearchResult result = JsonConvert.DeserializeObject<SearchResult>(json);

            return result;
        }

        private string ConstructSearchUri(SearchRequest request)
        {
            var uriQuery = endpoint + "?q=" + Uri.EscapeDataString(request.SearchTerm);
            uriQuery = uriQuery + "&count=" + request.ResultsPerPage;
            uriQuery = uriQuery + "&offset=" + ((request.StartingPage - 1) * request.ResultsPerPage);

            if (request.CountryCode == "All" || request.CountryCode == null) return uriQuery;

            uriQuery = uriQuery + "&mkt=" + request.CountryCode;

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

        private bool UriContainsBlackListTerm(WebPagesValue webPage)
        {
            return _blackListTermsList.Any(s => webPage.Url.AbsoluteUri.Contains(s));
        }

        private bool UriContainsWhiteListTld(WebPagesValue webPage)
        {
            return _whiteListTldList.Any(s => webPage.Url.AbsoluteUri.Contains(s));
        }

        private bool UriEndsWithWhiteListTld(WebPagesValue webPage)
        {
            return _whiteListTldList.Any(s => webPage.Url.AbsoluteUri.Contains(s));
        }

        private string CleanUri(SettingResponse settings, string uri, WebPagesValue webPage)
        {
            if (uri.IndexOf('?') > -1)
            {
                uri = uri.Substring(0, uri.IndexOf('?'));
            }

            if (UriEndsWithWhiteListTld(webPage)) return uri;

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

        private HashSet<LeadItem> GetLeadItems(SettingResponse settings, IEnumerable<SearchResult> results)
        {
            HashSet<LeadItem> leads = new HashSet<LeadItem>();

            foreach (var result in results)
            {
                if (result.WebPages is not null)
                {
                    foreach (var webPage in result.WebPages.Value)
                    {
                        if (!UriContainsBlackListTerm(webPage) && UriContainsWhiteListTld(webPage))
                        {
                            LeadItem lead = new LeadItem();
                            lead.Name = webPage.Name;
                            lead.Url = webPage.Url.ToString();
                            lead.Host = webPage.Url.Host;
                            lead.ContactUrl = FindContactUrl(webPage);
                            leads.Add(lead);
                        }
                    }
                }
            }
            return leads = leads.GroupBy(elem => elem.Host).Select(group => group.First()).ToHashSet();
        }

        private async Task<HashSet<LeadItem>> GetPhoneNumber(HashSet<LeadItem> leads)
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.102 Safari/537.36 Edge/18.18363");
            HashSet<LeadItem> finalList = new HashSet<LeadItem>();
            Regex phoneNumExp = new Regex(@"(\({0,1}\d{3}\){0,1}[- \.]\d{3}[- \.]\d{4})|(\+\d{2}-\d{2,4}-\d{3,4}-\d{3,4})");
            foreach(var lead in leads)
            {
                string html = "";
                try
                {
                    var url = lead.ContactUrl ?? lead.Url;
                    html = await client.GetStringAsync(url);
                    int phoneIndex = html.IndexOf("phone");
                    
                    if (phoneIndex == -1)
                    {
                        phoneIndex = html.IndexOf("tel");
                    }

                    if (phoneIndex == -1)
                    {
                        string number = "COULDNT SCRAPE";
                        lead.Phone = number;
                        finalList.Add(lead);
                    }
                    else
                    {
                        var shortHtml = html.Substring(phoneIndex);
                        var match = phoneNumExp.Match(shortHtml);

                        if (!match.Success)
                        {
                            string number = "COULDNT SCRAPE";
                            lead.Phone = number;
                            finalList.Add(lead);
                        }
                        else
                        {
                            lead.Phone = match.Value;
                            finalList.Add(lead);
                        }
                    }
                }
                catch (Exception ex)
                {
                    string number = "COULDNT SCRAPE";
                    lead.Phone = number;
                }
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
