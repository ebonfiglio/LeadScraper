using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeadScraper.ViewModels
{
    public class SearchViewModel
    {
        public string SearchTerm { get; set; }

        public int ResultsPerPage { get; set; }

        public int StartingPage { get; set; }

        public int Pages { get; set; }

        public string CountryCode { get; set; }
    }
}
