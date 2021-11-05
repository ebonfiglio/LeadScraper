using LeadScraper.Domain.Contracts;
using LeadScraper.Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LeadScraper.Domain.Services
{
    public class WritefileService : IWriteFileService
    {
        public void WriteToFile(HashSet<LeadItem> leads)
        {
            using (StreamWriter file = new StreamWriter(@"C:\Users\Public\" + DateTime.Now.ToString("yyyyMMdd-HHmmss").Replace(@"\", "-") + "Leads.txt"))
            {
                file.WriteLine("Name               Website                           Phone");

                foreach (var lead in leads)
                {
                    file.WriteLine(lead.Name + "              " + lead.Url + "                               " + lead.Phone);
                }

            }
        }
    }
}
