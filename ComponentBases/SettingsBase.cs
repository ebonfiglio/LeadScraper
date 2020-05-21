using AutoMapper;
using LeadScraper.Domain.Contracts;
using LeadScraper.Domain.Helpers;
using LeadScraper.Domain.Models.Requests;
using LeadScraper.Domain.Models.Responses;
using LeadScraper.Infrastructure.Entities;
using LeadScraper.Models;
using LeadScraper.ViewModels;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeadScraper.ComponentBases
{
    public class SettingsBase : ComponentBase
    {
        [Inject]
        ISettingsService _settingsService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await InitializeSettings();
        }
        public SettingsViewModel Settings { get; set; } = new SettingsViewModel();

        public string AddedTld { get; set; }

        public string AddedTerm { get; set; }

        public ErrorModel Error { get; set; } = new ErrorModel(false);

        private int SettingId { get; set; } = 0;

        private async Task InitializeSettings()
        {

            var response = await _settingsService.GetAsync();
            if (response != null)
            {
                SettingId = response.Id;
                Settings.BingKey = response.BingKey;
                Settings.BlackListTerms = response.BlackListTerms?.Split(",").ToList();
                Settings.WhiteListTlds = response.WhiteListTlds?.Split(",").ToList();
            }
            else
            {
                Settings.BingKey = "";
                Settings.BlackListTerms = DefaultSettingsHelper.DefaultBlackListTerms().Split(",").ToList();
                Settings.WhiteListTlds = DefaultSettingsHelper.DefaultWhiteListTlds().Split(",").ToList();
                await AddInitialSettings(Settings);
                StateHasChanged();
            }
            
           
        }

        public async Task AddBlackListTerm(string term)
        {
            Settings.BlackListTerms.Add(term);
            await UpdateSettings(Settings);
        }

        public async Task AddWhiteListTld(string tld)
        {
            Settings.WhiteListTlds.Add(tld);
            await UpdateSettings(Settings);

        }

        public async Task AddBingKey(string bingKey)
        {
            Settings.BingKey = bingKey;
            await UpdateSettings(Settings);

        }

        public async Task DeleteTld(string tld)
        {
            Settings.WhiteListTlds.Remove(tld);
            await UpdateSettings(Settings);

        }
        public async Task DeleteTerm(string term)
        {
            Settings.BlackListTerms.Remove(term);
            await UpdateSettings(Settings);
        }

        public async Task SaveKey()
        {
            await UpdateSettings(Settings);
        }

        private async Task UpdateSettings(SettingsViewModel viewModel)
        {
            SettingRequest request = new SettingRequest();
            request.Id = SettingId;
            request.BingKey = Settings.BingKey;
            request.BlackListTerms = string.Join(",", Settings.BlackListTerms);
            request.WhiteListTlds = string.Join(",", Settings.WhiteListTlds);
            var result = await _settingsService.EditAsync(request);

            if (result == null)
            {
                Error.IsError = true;
                Error.ErrorMessage = "Error saving setting.";
                await InitializeSettings();
            }
           

        }

        private async Task AddInitialSettings(SettingsViewModel viewModel)
        {
            SettingRequest request = new SettingRequest();
            request.Id = SettingId;
            request.BingKey = Settings.BingKey;
            request.BlackListTerms = string.Join(",", Settings.BlackListTerms);
            request.WhiteListTlds = string.Join(",", Settings.WhiteListTlds);
            var result = await _settingsService.AddAsync(request);
            if (result != null)
            {
                SettingId = result.Id;
            }
        }
    }
}
