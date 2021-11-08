using LeadScraper.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadScraper.Infrastructure.Contracts
{
    public interface ISearchSettingsRepository
    {
        public SearchSetting Add(SearchSetting entity);

        public void Delete(SearchSetting entity);

        public SearchSetting Update(SearchSetting entity);

        public Task<SearchSetting> GetAsync();

        Task<bool> SaveChangesAsync();
    }
}
