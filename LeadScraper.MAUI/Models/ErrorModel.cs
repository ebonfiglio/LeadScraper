using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeadScraper.Models
{
    public class ErrorModel
    {
        public ErrorModel(bool isError)
        {
            IsError = isError;
        }

        public bool IsError { get; set; }

        public string ErrorMessage { get; set; }
    }
}
