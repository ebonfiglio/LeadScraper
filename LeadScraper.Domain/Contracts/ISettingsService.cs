using LeadScraper.Domain.Models.Requests;
using LeadScraper.Domain.Models.Responses;
using LeadScraper.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LeadScraper.Domain.Contracts
{
    public interface ISettingsService
    {
        Task<SettingResponse> GetAsync();
        Task<SettingResponse> AddAsync(SettingRequest request);
        Task<SettingResponse> EditAsync(SettingRequest request);
        void Delete(Setting request);
    }
}
