using LeadScraper.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LeadScraper.Infrastructure.Contracts
{
    public interface ISettingsRepository
    {
        public Setting Add(Setting entity);

        public void Delete(Setting entity);

        public Setting Update(Setting entity);

        public Task<Setting> GetAsync();
        Task<bool> SaveChangesAsync();
    }
}
