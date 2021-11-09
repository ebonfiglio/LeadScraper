using AutoMapper;
using LeadScraper.Domain.Contracts;
using LeadScraper.Domain.Models.Requests;
using LeadScraper.Domain.Models.Responses;
using LeadScraper.Infrastructure.Contracts;
using LeadScraper.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadScraper.Domain.Services
{
    public class SearchSettingService : ISearchSettingService
    {

        ISearchSettingsRepository _repo;
        IMapper _mapper;
        public SearchSettingService(ISearchSettingsRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<SearchSettingResponse> UpsertAsync(SearchRequest request)
        {
            if(await GetAsync() is not null) return await EditAsync(request);

            return await AddAsync(request);
        
        }

        private async Task<SearchSettingResponse> AddAsync(SearchRequest request)
        {
            var entity = _mapper.Map<SearchSetting>(request);
            entity = _repo.Add(entity);
            await _repo.SaveChangesAsync();
            return await Task.Run(() => _mapper.Map<SearchSettingResponse>(entity));
        }

        private async Task<SearchSettingResponse> EditAsync(SearchRequest request)
        {
            var entity = _repo.Update(_mapper.Map<SearchRequest, SearchSetting>(request));
            await _repo.SaveChangesAsync();

            return _mapper.Map<SearchSettingResponse>(entity);
        }

        public void Delete(SearchSetting request)
        {
            _repo.Delete(request);
        }

        public async Task<SearchSettingResponse> GetAsync()
        {
            var entity = await _repo.GetAsync();
            return _mapper.Map<SearchSettingResponse>(entity);
        }
    }
}
