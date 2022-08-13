using LeadScraper.Domain.Contracts;
using LeadScraper.Domain.Models;
using LeadScraper.Domain.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadScraper.Domain.Services
{
    public class GoogleSearch : ISearchService
    {
        public async Task<HashSet<LeadItem>> Search(SearchRequest request)
        {
            return new HashSet<LeadItem>();
        }
    }
}
