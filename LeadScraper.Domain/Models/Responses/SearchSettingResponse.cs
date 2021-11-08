using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadScraper.Domain.Models.Responses
{
    public class SearchSettingResponse
    {
        public string SearchTerm { get; set; }

        public int ResultsPerPage { get; set; }

        public int StartingPage { get; set; }

        public int Pages { get; set; }

        public string CountryCode { get; set; }
    }
}
