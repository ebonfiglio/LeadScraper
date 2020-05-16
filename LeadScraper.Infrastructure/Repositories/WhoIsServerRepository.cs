using LeadScraper.Infrastructure.Contracts;
using LeadScraper.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LeadScraper.Infrastructure.Repositories
{
    public class WhoIsServerRepository : IWhoIsServerRepository
    {
        ApplicationDbContext _context;
        public WhoIsServerRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<WhoIsServer> GetAll()
        {
            return _context.WhoIsServers.ToList();
        }
    }
}
