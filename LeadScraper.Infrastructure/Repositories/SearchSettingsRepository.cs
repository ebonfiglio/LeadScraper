using LeadScraper.Infrastructure.Contracts;
using LeadScraper.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadScraper.Infrastructure.Repositories
{
    public class SearchSettingsRepository : ISearchSettingsRepository
    {
        ApplicationDbContext _context;

        public SearchSettingsRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public SearchSetting Add(SearchSetting entity)
        {
            _context.SearchSetting.Add(entity);
            return entity;
        }

        public void Delete(SearchSetting entity)
        {
            _context.SearchSetting.Remove(entity);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<SearchSetting> GetAsync()
        {
            return await _context.SearchSetting.AsNoTracking().FirstOrDefaultAsync();
        }

        public SearchSetting Update(SearchSetting entity)
        {
            _context.SearchSetting.Update(entity);
            return entity;
        }
    }
}
