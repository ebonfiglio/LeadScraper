using AutoMapper;
using LeadScraper.Domain.Contracts;
using LeadScraper.Domain.Helpers;
using LeadScraper.Domain.Models.Requests;
using LeadScraper.Domain.Models.Responses;
using LeadScraper.Infrastructure.Contracts;
using LeadScraper.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LeadScraper.Domain.Services
{
    public class SettingsService : ISettingsService
    {
        ISettingsRepository _repo;
        IMapper _mapper;
        public SettingsService(ISettingsRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        public async Task<SettingResponse> AddAsync(SettingRequest request)
        {
            var entity = _mapper.Map<Setting>(request);
            entity = _repo.Add(entity);
            await _repo.SaveChangesAsync();
            return await Task.Run(()=>_mapper.Map<SettingResponse>(entity));        
        }

        public void Delete(Setting request)
        {
            //_repo.Delete(_mapper.Map<Setting>(request));
            _repo.Delete(request);
        }

        public async Task<SettingResponse> EditAsync(SettingRequest request)
        {

           var entity = _repo.Update(_mapper.Map<SettingRequest, Setting>(request));
           await _repo.SaveChangesAsync();

           return _mapper.Map<SettingResponse>(entity);
        }

        public async Task<SettingResponse> GetAsync()
        {
            var entity = await _repo.GetAsync();
            return _mapper.Map<SettingResponse>(entity);
        }
    }
}
