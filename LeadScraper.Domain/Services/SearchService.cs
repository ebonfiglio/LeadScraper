using LeadScraper.Domain.Contracts;
using LeadScraper.Domain.Models.Requests;
using LeadScraper.Domain.Models.Responses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace LeadScraper.Domain.Services
{
    public class SearchService : ISearchService
    {
        ISettingsService _settingsService;
        const string endpoint = "https://api.cognitive.microsoft.com/bing/v7.0/search";
        public SearchService(ISettingsService settingsService )
        {
            _settingsService = settingsService;
        }
        public async void Search(SearchRequest request)
        {
            var settings = await _settingsService.GetAsync();

        }

        private static string GetBingResults(SearchRequest request, SettingResponse settings)
        {
            string uriQuery = ConstructSearchUri(request);

            return GetSearchResultJson(uriQuery, settings);


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
    }
}
