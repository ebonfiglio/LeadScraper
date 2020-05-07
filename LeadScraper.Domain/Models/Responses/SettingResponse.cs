using System;
using System.Collections.Generic;
using System.Text;

namespace LeadScraper.Domain.Models.Responses
{
    public class SettingResponse
    {
        public int Id { get; set; }
        public string WhiteListTlds { get; set; }

        public string BlackListTerms { get; set; }

        public string BingKey { get; set; }
    }
}
