using LeadScraper.Domain.Contracts;
using LeadScraper.Domain.Models;
using LeadScraper.Domain.Models.Requests;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LeadScraper.Domain.Services
{
    public class WritefileService : IWriteFileService
    {
        public void WriteToFile(HashSet<LeadItem> leads, SearchRequest searchRequest)
        {
            using (StreamWriter file = new StreamWriter(@"C:\Users\Public\" + DateTime.Now.ToString("yyyyMMdd-HHmmss").Replace(@"\", "-") + "Leads.txt"))
            {
                file.WriteLine($"Search Term: {searchRequest.SearchTerm}");
                file.WriteLine($"Country Code: {searchRequest.CountryCode}");
                file.WriteLine($"Starting Page: {searchRequest.StartingPage}");
                file.WriteLine($"Pages: {searchRequest.Pages}");
                file.WriteLine($"Results Per Page: {searchRequest.ResultsPerPage}");

                file.WriteLine("Name               Website                           Phone");

                foreach (var lead in leads)
                {
                    file.WriteLine(lead.Name + "              " + lead.Url + "                               " + lead.Phone);
                }

            }
        }
    }
}
