using System;
using System.Collections.Generic;
using System.Text;

namespace LeadScraper.Domain.Models.Requests
{
    public class SearchRequest
    {
        public int Id { get; set; }
        public string SearchTerm { get; set; }

        public int ResultsPerPage { get; set; }

        public int StartingPage { get; set; }

        public int Pages { get; set; }

        public string CountryCode { get; set; }
    }
}
