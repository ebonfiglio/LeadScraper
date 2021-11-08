using LeadScraper.Domain.Models.Requests;
using LeadScraper.Domain.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadScraper.Domain.Contracts
{
    public interface ISeachSettingService
    {
        Task<SearchSettingResponse> GetAsync();
        Task<SearchSettingResponse> AddAsync(SearchRequest request);
        Task<SearchSettingResponse> EditAsync(SearchRequest request);
        void Delete(SearchSettingResponse request);
    }
}
