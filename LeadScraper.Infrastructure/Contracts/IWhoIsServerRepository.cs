using LeadScraper.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeadScraper.Infrastructure.Contracts
{
    public interface IWhoIsServerRepository
    {
        List<WhoIsServer> GetAll();
    }
}
