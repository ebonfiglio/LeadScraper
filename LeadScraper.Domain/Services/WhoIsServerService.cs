using AutoMapper;
using LeadScraper.Domain.Contracts;
using LeadScraper.Domain.Models.Responses;
using LeadScraper.Infrastructure.Contracts;
using LeadScraper.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LeadScraper.Domain.Services
{
    public class WhoIsServerService : IWhoIsServerService
    {
        IWhoIsServerRepository _whoIsServerRepository;
        IMapper _mapper;
        public WhoIsServerService(IWhoIsServerRepository whoIsServerRepository, IMapper mapper)
        {
            _whoIsServerRepository = whoIsServerRepository;
            _mapper = mapper;
        }
        public List<WhoIsServerResponse> GetAll()
        {
            List<WhoIsServerResponse> responseList = _whoIsServerRepository.GetAll().Select(l => new WhoIsServerResponse() { Tld = l.Tld, Server = l.Server }).ToList();
            return responseList;
        }

    }
}
