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
            return await Task.Run(()=>_mapper.Map<SettingResponse>(_repo.Add(entity)));        
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

           return _mapper.Map<SettingResponse>(_repo.Add(entity));
        }

        public async Task<SettingResponse> GetAsync()
        {
            var entity = await _repo.GetAsync();
            if(entity == null)
            {
                entity = await SeedInitialSetting();
            }
            return _mapper.Map<SettingResponse>(entity);
        }

        private async Task<Setting> SeedInitialSetting()
        {
            SettingRequest request = new SettingRequest() { BingKey = "", BlackListTerms = DefaultSettingsHelper.DefaultBlackListTerms(), WhiteListTlds = DefaultSettingsHelper.DefaultWhiteListTlds() };
            var response = await AddAsync(request);
            await _repo.SaveChangesAsync();
            return _mapper.Map<Setting>(response);
        }
    }
}
