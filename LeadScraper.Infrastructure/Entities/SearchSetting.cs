using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadScraper.Infrastructure.Entities
{
    public class SearchSetting
    {
        [Key]
        public int Id { get; set; }
        public string SearchTerm { get; set; }
        public int ResultsPerPage { get; set; }
        public int StartingPage { get; set; }
        public int Pages { get; set; }
        public string CountryCode { get; set; }
    }
}
