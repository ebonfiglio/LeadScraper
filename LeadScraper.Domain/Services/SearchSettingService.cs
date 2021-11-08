using AutoMapper;
using LeadScraper.Domain.Contracts;
using LeadScraper.Domain.Models.Requests;
using LeadScraper.Domain.Models.Responses;
using LeadScraper.Infrastructure.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadScraper.Domain.Services
{
    public class SearchSettingService : ISeachSettingService
    {

        ISearchSettingsRepository _repo;
        IMapper _mapper;
        public SearchSettingService(ISearchSettingsRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        public async Task<SearchSettingResponse> AddAsync(SettingRequest request)
        {
            var entity = _mapper.Map<Setting>(request);
            entity = _repo.Add(entity);
            await _repo.SaveChangesAsync();
            return await Task.Run(() => _mapper.Map<SettingResponse>(entity));
        }

        public void Delete(Setting request)
        {
            //_repo.Delete(_mapper.Map<Setting>(request));
            _repo.Delete(request);
        }

        public async Task<SearchSettingResponse> EditAsync(SettingRequest request)
        {

            var entity = _repo.Update(_mapper.Map<SettingRequest, Setting>(request));
            await _repo.SaveChangesAsync();

            return _mapper.Map<SettingResponse>(entity);
        }

        public async Task<SearchSettingResponse> GetAsync()
        {
            var entity = await _repo.GetAsync();
            return _mapper.Map<SettingResponse>(entity);
        }
    }
}
