using LeadScraper.Domain.Models;
using LeadScraper.Domain.Models.Requests;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LeadScraper.Domain.Contracts
{
    public interface ISearchService
    {
         Task<HashSet<LeadItem>> Search(SearchRequest request);
    }
}
