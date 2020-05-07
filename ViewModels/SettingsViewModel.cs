using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace LeadScraper.ViewModels
{
    public class SettingsViewModel
    {
        public string BingKey { get; set; }

        public List<string> BlackListTerms { get; set; }

        public List<string> WhiteListTlds{ get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
