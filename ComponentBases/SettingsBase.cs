using AutoMapper;
using LeadScraper.Domain.Contracts;
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
    public class SettingsBase :ComponentBase
    {
        [Inject]
        ISettingsService _settingsService { get; set; }

        protected override Task OnInitializedAsync()
        {
            InitializeSettings();
            return  base.OnInitializedAsync();
        }
        public SettingsViewModel Settings { get; set; } = new SettingsViewModel();

        public ErrorModel Error { get; set; } = new ErrorModel(false);

        private int SettingId { get; set; }

        private async void InitializeSettings()
        {
            var response = await _settingsService.GetAsync();
            SettingId = response.Id;
            Settings.BingKey = response.BingKey;
            Settings.BlackListTerms = response.BlackListTerms?.Split(",").ToList();
            Settings.WhiteListTlds = response.WhiteListTlds?.Split(",").ToList();
            if(SettingId == 0)
            {
                StateHasChanged();
            }
        }

        public void AddBlackListTerm(string term)
        {
            Settings.BlackListTerms.Add(term);
            UpdateSettings(Settings);
        }

        public void AddWhiteListTld(string tld)
        {
            Settings.WhiteListTlds.Add(tld);
            UpdateSettings(Settings);

        }

        public void AddBingKey(string bingKey)
        {
            Settings.BingKey = bingKey;
            UpdateSettings(Settings);

        }

        public void DeleteTld(string tld)
        {
            Settings.WhiteListTlds.Remove(tld);
            UpdateSettings(Settings);
             
        }
        public void DeleteTerm(string term)
        {
            Settings.BlackListTerms.Remove(term);
            UpdateSettings(Settings);
        }

        private async void UpdateSettings(SettingsViewModel viewModel)
        {
            SettingRequest request = new SettingRequest();
            request.Id = SettingId;
            request.BingKey = Settings.BingKey;
            request.BlackListTerms = string.Join(",", Settings.BlackListTerms);
            request.WhiteListTlds = string.Join(",", Settings.WhiteListTlds);
            var result  = await _settingsService.EditAsync(request);
            if(result == null)
            {
                Error.IsError = true;
                Error.ErrorMessage = "Error saving saving.";
                InitializeSettings();
            }
        }
    }
}
