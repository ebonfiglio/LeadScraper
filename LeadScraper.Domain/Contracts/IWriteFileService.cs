using LeadScraper.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeadScraper.Domain.Contracts
{
    public interface IWriteFileService
    {
        void WriteToFile(List<LeadItem> leads);
    }
}
