using LeadScraper.Domain.Contracts;
using LeadScraper.Domain.Helpers;
using LeadScraper.Domain.Models.Requests;
using LeadScraper.Models;
using MatBlazor;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeadScraper.ViewModels
{
    public class SettingsViewModel : ComponentBase
    {
        [Inject]
        ISettingsService _settingsService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await InitializeSettings();
        }
        public string BingKey { get; set; }

        public List<string> BlackListTerms { get; set; }

        public List<string> WhiteListTlds { get; set; }

        public string AddedTld { get; set; }

        public string AddedTerm { get; set; }

        public MatChipSet chipsetTerms = null;

        public MatChipSet chipsetTlds = null;

        public ErrorModel Error { get; set; } = new ErrorModel(false);

        private int SettingId { get; set; } = 0;

        private async Task InitializeSettings()
        {

            var response = await _settingsService.GetAsync();
            if (response != null)
            {
                SettingId = response.Id;
                BingKey = response.BingKey;
                BlackListTerms = response.BlackListTerms?.Split(",").ToList();
                WhiteListTlds = response.WhiteListTlds?.Split(",").ToList();
            }
            else
            {
                BingKey = "";
                BlackListTerms = DefaultSettingsHelper.DefaultBlackListTerms().Split(",").ToList();
                WhiteListTlds = DefaultSettingsHelper.DefaultWhiteListTlds().Split(",").ToList();
                await AddInitialSettings();
                StateHasChanged();
            }
            
           
        }

        public async Task AddBlackListTerm(string term)
        {
            BlackListTerms.Add(term);
            await UpdateSettings();
        }

        public async Task AddWhiteListTld(string tld)
        {
            WhiteListTlds.Add(tld);
            await UpdateSettings();
        }

        public async Task AddBingKey(string bingKey)
        {
            BingKey = bingKey;
            await UpdateSettings();

        }

        public async Task DeleteTld(MatChip chip)
        {
            WhiteListTlds.Remove(chip.Label);
            chipsetTerms?.UnregisterChip(chip);
            await UpdateSettings();
        }
        public async Task DeleteTerm(MatChip chip)
        {
            BlackListTerms.Remove(chip.Label);
            chipsetTerms?.UnregisterChip(chip);
            await UpdateSettings();
        }

        public async Task SaveKey()
        {
            await UpdateSettings();
        }

        private async Task UpdateSettings()
        {
            SettingRequest request = new SettingRequest();
            request.Id = SettingId;
            request.BingKey = BingKey;
            request.BlackListTerms = string.Join(",", BlackListTerms);
            request.WhiteListTlds = string.Join(",", WhiteListTlds);
            var result = await _settingsService.EditAsync(request);

            if (result == null)
            {
                Error.IsError = true;
                Error.ErrorMessage = "Error saving setting.";
                await InitializeSettings();
            }
        }

        private async Task AddInitialSettings()
        {
            SettingRequest request = new SettingRequest();
            request.Id = SettingId;
            request.BingKey = BingKey;
            request.BlackListTerms = string.Join(",", BlackListTerms);
            request.WhiteListTlds = string.Join(",", WhiteListTlds);
            var result = await _settingsService.AddAsync(request);
            if (result != null)
            {
                SettingId = result.Id;
            }
        }
    }
}
