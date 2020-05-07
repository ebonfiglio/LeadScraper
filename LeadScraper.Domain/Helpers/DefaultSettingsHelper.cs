using System;
using System.Collections.Generic;
using System.Text;

namespace LeadScraper.Domain.Helpers
{
    public class DefaultSettingsHelper
    {

        public static string DefaultBlackListTerms()
        {
            return "wiki,news,journal,indeed,amazon,alibaba,forbes,yahoo,aol,bloomberg";

        }
        public static string DefaultWhiteListTlds()
        {
            return".com";
         
        }
    }
}
