using LeadScraper.Domain.Models.Requests;
using LeadScraper.Domain.Models.Responses;
using LeadScraper.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadScraper.Domain.Contracts
{
    public interface ISearchSettingService
    {
        Task<SearchSettingResponse> GetAsync();
        Task<SearchSettingResponse> UpsertAsync(SearchRequest request);
        void Delete(SearchSetting request);
    }
}
