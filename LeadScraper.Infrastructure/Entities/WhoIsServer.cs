using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LeadScraper.Infrastructure.Entities
{
    public class WhoIsServer
    {
        [Key]
        public int Id { get; set; }
        public string Tld { get; set; }

        public string Server { get; set; }
    }
}
