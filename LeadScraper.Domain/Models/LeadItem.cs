using System;
using System.Collections.Generic;
using System.Text;

namespace LeadScraper.Domain.Models
{
    public class LeadItem
    {
            public string Name { get; set; }

            public string Url { get; set; }

            public string ContactUrl { get; set; }
            public string Phone { get; set; }

            public string AbsoluteUri { get; set; }
    }
}
