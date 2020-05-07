using AutoMapper;
using LeadScraper.Domain.Models.Requests;
using LeadScraper.Domain.Models.Responses;
using LeadScraper.Infrastructure.Entities;

namespace LeadScraper.Domain.Mapper
{
    public class LeadScraperProfile : Profile
    {
        public LeadScraperProfile()
        {
            CreateMap<SettingRequest, Setting>().ReverseMap();
            CreateMap<SettingResponse, Setting>().ReverseMap();
        }
       
    }
}
