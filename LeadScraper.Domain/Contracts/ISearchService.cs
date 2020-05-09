using LeadScraper.Domain.Models.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeadScraper.Domain.Contracts
{
    public interface ISearchService
    {
        void Search(SearchRequest request);
    }
}
