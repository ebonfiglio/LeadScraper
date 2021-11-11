using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadScraper.Domain.Contracts
{
    public interface ILoggingService
    {
        void LogError(Exception ex);
    }
}
