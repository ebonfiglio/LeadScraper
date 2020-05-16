using LeadScraper.Domain.Models.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeadScraper.Domain.Contracts
{
    public interface IWhoIsServerService
    {

        List<WhoIsServerResponse> GetAll();
    }
}
