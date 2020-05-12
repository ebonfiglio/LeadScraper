using LeadScraper.Domain.Contracts;
using LeadScraper.Domain.Models;
using LeadScraper.Domain.Models.Requests;
using LeadScraper.Domain.Models.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace LeadScraper.Domain.Services
{
    public class SearchService : ISearchService
    {
        ISettingsService _settingsService;
        const string endpoint = "https://api.cognitive.microsoft.com/bing/v7.0/search";
        public SearchService(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }
        public async void Search(SearchRequest request)
        {
            var settings = await _settingsService.GetAsync();
            SearchResult result = GetBingSearchResult(request, settings);
            List<LeadItem> leads = GetLeadItems(settings, result);


        }


        private static SearchResult GetBingSearchResult(SearchRequest request, SettingResponse settings)
        {
            string uriQuery = ConstructSearchUri(request);
            string json = GetSearchResultJson(uriQuery, settings);
            SearchResult result = JsonConvert.DeserializeObject<SearchResult>(json);
           
            return result;


        }

        private static string ConstructSearchUri(SearchRequest request)
        {
            var uriQuery = endpoint + "?q=" + Uri.EscapeDataString(request.SearchTerm);
            uriQuery = uriQuery + "&count=" + request.ResultsPerPage;
            uriQuery = uriQuery + "&offset=" + ((request.StartingPage - 1) * request.ResultsPerPage);

            if (request.CountryCode == "All") return uriQuery;

            uriQuery = uriQuery + "&cc=" + request.CountryCode;

            return uriQuery;
        }

        private static string GetSearchResultJson(string uri, SettingResponse settings)
        {
            WebRequest webRequest = HttpWebRequest.Create(uri);
            webRequest.Headers["Ocp-Apim-Subscription-Key"] = settings.BingKey;
            HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponseAsync().Result;
            string json = new StreamReader(webResponse.GetResponseStream()).ReadToEnd();

            return json;
        }

        private static bool UriContainsBlackListTerm(SettingResponse settings, WebPagesValue webPage)
        {
            List<string> blackListTermsList = new List<string>(settings.BlackListTerms.Split(','));
            return blackListTermsList.Any(s => webPage.Url.AbsoluteUri.Contains(s));
        }

        private static bool UriContainsWhiteListTld(SettingResponse settings, WebPagesValue webPage)
        {
            List<string> whiteListTldList = new List<string>(settings.WhiteListTlds.Split(','));
            return whiteListTldList.Any(s => webPage.Url.AbsoluteUri.Contains(s));
        }

        private static bool UriEndsWithWhiteListTld(SettingResponse settings, WebPagesValue webPage)
        {
            List<string> whiteListTldList = new List<string>(settings.WhiteListTlds.Split(','));
            return whiteListTldList.Any(s => webPage.Url.AbsoluteUri.EndsWith(s)) || whiteListTldList.Any(s => webPage.Url.AbsoluteUri.EndsWith(s + "/"));
        }

        private static string CleanUri(SettingResponse settings, string uri, WebPagesValue webPage)
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

        private static string FindContactUrl(WebPagesValue webPage)
        {
            return webPage.DeepLinks?.FirstOrDefault(l => l.Name.Contains("Contact"))?.Url?.ToString();
            
        }

        private static List<LeadItem> GetLeadItems(SettingResponse settings, SearchResult result)
        {
            List<LeadItem> leads = new List<LeadItem>();
            foreach (var webPage in result.WebPages.Value)
            {
                if (!UriContainsBlackListTerm(settings, webPage) && UriContainsWhiteListTld(settings, webPage))
                {
                    LeadItem lead = new LeadItem();
                    lead.Name = webPage.Name;
                    lead.Url = webPage.Url.ToString();
                    lead.AbsoluteUri = CleanUri(settings, webPage.Url.AbsoluteUri, webPage);
                    lead.ContactUrl = FindContactUrl(webPage);
                    leads.Add(lead);

                }

            }
            return leads = leads.GroupBy(elem => elem.AbsoluteUri).Select(group => group.First()).ToList();
        }
    }
}
