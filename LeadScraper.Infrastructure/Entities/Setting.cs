using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LeadScraper.Infrastructure.Entities
{
    public class Setting
    {
        [Key]
        public int Id { get; set; }
        public string WhiteListTlds { get; set; }

        public string BlackListTerms { get; set; }

        public string BingKey { get; set; }
    }
}
