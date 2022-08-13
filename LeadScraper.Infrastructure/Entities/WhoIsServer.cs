using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LeadScraper.Infrastructure.Entities
{
    public class WhoIsServer
    {
        [Key]
        public string Tld { get; set; }

        public string Server { get; set; }
    }
}
