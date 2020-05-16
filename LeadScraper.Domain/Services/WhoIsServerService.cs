using AutoMapper;
using LeadScraper.Domain.Contracts;
using LeadScraper.Domain.Models.Responses;
using LeadScraper.Infrastructure.Contracts;
using LeadScraper.Infrastructure.Entities;
using System;
using System.Collections.Generic;
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
            return _mapper.Map<List<WhoIsServerResponse>>(_whoIsServerRepository.GetAll());
        }

    }
}
