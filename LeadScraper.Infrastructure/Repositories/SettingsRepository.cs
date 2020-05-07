using LeadScraper.Infrastructure.Contracts;
using LeadScraper.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LeadScraper.Infrastructure.Repositories
{
    public class SettingsRepository : ISettingsRepository
    {

        ApplicationDbContext _context;

        public SettingsRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public Setting Add(Setting entity)
        {
            _context.Settings.Add(entity);
            return entity;
        }

        public void Delete(Setting entity)
        {
            _context.Settings.Remove(entity);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Setting> GetAsync()
        {
            return await _context.Settings.AsNoTracking().FirstOrDefaultAsync();
        }

        public Setting Update(Setting entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return entity;
        }
    }
}
