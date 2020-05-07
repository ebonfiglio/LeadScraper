using System;
using System.Collections.Generic;
using System.Text;

namespace LeadScraper.Domain.Models.Requests
{
    public class SettingRequest
    {
        public int Id { get; set; }
        public string WhiteListTlds { get; set; }

        public string BlackListTerms { get; set; }

        public string BingKey { get; set; }
    }
}
