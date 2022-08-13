using LeadScraper.Domain.Contracts;
using LeadScraper.Domain.Models.Requests;
using LeadScraper.Models;
using LeadScraper.MAUI.Shared;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeadScraper.ViewModels
{
    public class SearchViewModel :ComponentBase
    {
        [Inject]
        ISearchService _searchService { get; set; }

        [Inject]
        ILoggingService _loggingService { get; set; }

        [Inject]
        ISearchSettingService _searchSettingService { get; set; }

        [Inject]
        IWriteFileService _writeFileService { get; set; }

        public string SearchTerm { get; set; }
        public int ResultsPerPage { get; set; } = 10;
        public int StartingPage { get; set; } = 1;
        public int Pages { get; set; } = 1;
        public string CountryCode { get; set; } = "All";
        public List<CountryModel> CountryList { get; set; }

        protected SearchResultComponent ChildComponent;

        public int LeadsFound { get; set; }

        protected override Task OnInitializedAsync()
        {
            InitializeSettings();
            return base.OnInitializedAsync();
        }
        private async void InitializeSettings()
        {
            var countries = JsonConvert.DeserializeObject<List<CountryModel>>(GetCountryCodes());
            CountryList = countries;

            var searchSettings = await _searchSettingService.GetAsync();
            if(searchSettings != null)
            {
                ResultsPerPage = searchSettings.ResultsPerPage;
                StartingPage = searchSettings.StartingPage;
                Pages = searchSettings.Pages;
                CountryCode = searchSettings.CountryCode;
            }
        }

        private string GetCountryCodes()
        {
            return @"[
{ 'name': 'All', 'code': 'All'},
{ 'name': 'Argentina (Spanish)', 'code': 'es-AR'},
{ 'name': 'Australia (English)', 'code': 'en-AU'},
{ 'name': 'Austria (German)', 'code': 'de-AT'},
{ 'name': 'Belgium (Dutch)', 'code': 'nl-BE'},
{ 'name': 'Belgium (French)', 'code': 'fr-BE'},
{ 'name': 'Brazil (Portuguese)', 'code': 'pt-BR'},
{ 'name': 'Canada (English)', 'code': 'en-CA'},
{ 'name': 'Canada (French)', 'code': 'fr-CA'},
{ 'name': 'Chile (Spanish)', 'code': 'es-CL'},
{ 'name': 'Denmark (Danish)', 'code': 'da-DK'},
{ 'name': 'Finland (Finnish)', 'code': 'fi-FI'},
{ 'name': 'France (French)', 'code': 'fr-FR'},
{ 'name': 'Germany (German)', 'code': 'de-DE'},
{ 'name': 'Hong Kong SAR (Traditional Chinese)', 'code': 'zh-HK'},
{ 'name': 'India (English)', 'code': 'en-IN'},
{ 'name': 'Indonesia (English)', 'code': 'en-ID'},
{ 'name': 'Italy (Italian)', 'code': 'it-IT'},
{ 'name': 'Japan (Japanese)', 'code': 'ja-JP'},
{ 'name': 'Korea (Korean)', 'code': 'ko-KR'},
{ 'name': 'Malaysia (English)', 'code': 'en-MY'},
{ 'name': 'Mexico (Spanish)', 'code': 'es-MX'},
{ 'name': 'Netherlands (Dutch)', 'code': 'nl-NL'},
{ 'name': 'New Zealand (English)', 'code': 'en-NZ'},
{ 'name': 'Norway (Norwegian)', 'code': 'no-NO'},
{ 'name': 'Peoples republic of China (Chinese)', 'code': 'zh-CN'},
{ 'name': 'Poland (Polish)', 'code': 'pl-PL'},
{ 'name': 'Republic of the Philippines (English)', 'code': 'en-PH'},
{ 'name': 'Russia (Russian)', 'code': '	ru-RU'},
{ 'name': 'South Africa (English)', 'code': 'en-ZA'},
{ 'name': 'Spain (Spanish)', 'code': 'es-ES'},
{ 'name': 'Sweden (Swedish)', 'code': 'sv-SE'},
{ 'name': 'Switzerland (French)', 'code': 'fr-CH'},
{ 'name': 'Switzerland (German)', 'code': 'de-CH'},
{ 'name': 'Taiwan (Traditional Chinese)', 'code': 'zh-TW'},
{ 'name': 'Turkey (Turkish)', 'code': 'tr-TR'},
{ 'name': 'United Kingdom (English)', 'code': 'en-GB'},
{ 'name': 'United States (English)', 'code': 'en-US'},
{ 'name': 'United States (Spanish)', 'code': 'es-US'}
]";
        }

        public async Task GetLeads()
        {
            try
            {
                SearchRequest searchRequest = new SearchRequest();
                searchRequest.CountryCode = CountryCode;
                searchRequest.Pages = Pages;
                searchRequest.ResultsPerPage = ResultsPerPage;
                searchRequest.SearchTerm = SearchTerm;
                searchRequest.StartingPage = StartingPage;
                searchRequest.Id = 1;

                ChildComponent.Refresh(true, 0, null);
                await Task.Yield();

                var leads = await _searchService.Search(searchRequest);
                _writeFileService.WriteToFile(leads, searchRequest);
                LeadsFound = leads.Count();
                ChildComponent.Refresh(false, LeadsFound, leads.ToList());

                await _searchSettingService.UpsertAsync(searchRequest);
            }
            catch(Exception ex)
            {
                _loggingService.LogError(ex);
                throw;
            }
        }
    }
}
