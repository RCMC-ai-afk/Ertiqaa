using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Orbits.GeneralProject.BLL.BaseReponse;
using Orbits.GeneralProject.BLL.ImprovementSuggestionsService;
using Orbits.GeneralProject.BLL.SharedService;
using Orbits.GeneralProject.BLL.StaticEnums;
using Orbits.GeneralProject.BLL.Validation.ImprovementOpportunityValidation;
using Orbits.GeneralProject.BLL.Validation.RoadValidation;
using Orbits.GeneralProject.Core.Entities;
using Orbits.GeneralProject.Core.Infrastructure;
using Orbits.GeneralProject.DTO;
using Orbits.GeneralProject.DTO.DepartmentDtos;
using Orbits.GeneralProject.DTO.ImprovementOpportunityDtos;
using Orbits.GeneralProject.DTO.Paging;
using Orbits.GeneralProject.DTO.RoadDtos;
using Orbits.GeneralProject.Repositroy.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbits.GeneralProject.BLL.ImprovementOpportunityService
{
    public class ImprovementOpportunityBLL :BaseBLL, IImprovementOpportunityBLL
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<ImprovementOpportunity> _improvementOpportunityRepository;
        private readonly IImprovementSuggestionsBLL _improvementSuggestionsBLL;
        private readonly IRepository<User> _userRepository;
        public ImprovementOpportunityBLL(IMapper mapper,
            IUnitOfWork unitOfWork,
            IRepository<ImprovementOpportunity> improvementOpportunityRepository,
            IRepository<User> userRepository,
            IImprovementSuggestionsBLL improvementSuggestionsBLL) : base(mapper)
        {
            _improvementOpportunityRepository = improvementOpportunityRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _improvementSuggestionsBLL = improvementSuggestionsBLL;
        }
        public IResponse<PagedResultDto<ImprovementOpportunityViewModel>> GetPagedList(ImprovementOpportunityFilteredDto pagedDto,int userId)
        {
            string? searchWord = pagedDto.SearchTerm?.ToLower().Trim();
            Response<PagedResultDto<ImprovementOpportunityViewModel>> output = new Response<PagedResultDto<ImprovementOpportunityViewModel>>();
            PagedResultDto<ImprovementOpportunityViewModel> list = GetPagedList<ImprovementOpportunityViewModel, ImprovementOpportunity, int>(pagedDto, repository: _improvementOpportunityRepository, x => x.Id, 
                searchExpression: x =>
                   (pagedDto.OpportunityTypeIds==null || pagedDto.OpportunityTypeIds!=null && pagedDto.OpportunityTypeIds.Contains(x.OpportunityTypeId.Value)) &&
                   (pagedDto.OpportunityStatusIds==null || pagedDto.OpportunityStatusIds !=null && pagedDto.OpportunityStatusIds.Contains(x.OpportunityStatusId.Value)) &&
                   string.IsNullOrEmpty(searchWord) ||
                   (!string.IsNullOrEmpty(searchWord) && x.Name.Contains(searchWord) || x.OpportunityStatus.Name.Contains(searchWord)),
                   sortDirection: "DESC",
              disableFilter: true,
              excluededColumns: null
              );
            return output.CreateResponse(list);
        }
        public async Task<IResponse<bool>> AddAsync(ImprovementOpportunityCreateDto dto,int userId)
        {
            Response<bool> output = new Response<bool>();
            ImprovementOpportunityCreateValidation validator = new ImprovementOpportunityCreateValidation();
            ValidationResult validationResult = validator.Validate(dto);
            if (!validationResult.IsValid)
                return output.AppendErrors(validationResult.Errors);
            ImprovementOpportunity entity = _mapper.Map<ImprovementOpportunityCreateDto, ImprovementOpportunity>(dto);
            entity.OpportunityStatusId = (int)OpportunityStatusEnum.New;
            entity.CreatedAt= DateTime.Now;
            entity.CreatedByUserId = userId;
            _improvementOpportunityRepository.Add(entity);
            await _unitOfWork.CommitAsync();
            return output.CreateResponse(true);
        }
        public async Task<IResponse<ImprovementOpportunityGetByIdDto>> GetById(int id, int userId)
        {
            Response<ImprovementOpportunityGetByIdDto> output = new Response<ImprovementOpportunityGetByIdDto>();
            ImprovementOpportunity entity = await _improvementOpportunityRepository.GetByIdAsync(id);
            if (entity == null)
                return output.CreateResponse(Constants.MessageCodes.NotFound);
            return output.CreateResponse(_mapper.Map<ImprovementOpportunityGetByIdDto>(entity));
        }
        public async Task<IResponse<bool>> UpdateAsync(ImprovementOpportunityUpdateDto dto, int userId)
        {
            Response<bool> output = new Response<bool>();
            ImprovementOpportunityCreateValidation validator = new ImprovementOpportunityCreateValidation();
            ValidationResult validationResult = validator.Validate(dto);
            if (!validationResult.IsValid)
                return output.AppendErrors(validationResult.Errors);
            ImprovementOpportunity entity = await _improvementOpportunityRepository.GetByIdAsync(dto.Id);
            if (entity == null)
                return output.CreateResponse(Constants.MessageCodes.NotFound);
            _mapper.Map(dto, entity);
            await _unitOfWork.CommitAsync();
            return output.CreateResponse(true);
        }
        public async Task<IResponse<bool>> DeleteAsync(int id, int userId)
        {
            Response<bool> output = new Response<bool>();
            ImprovementOpportunity entity = await _improvementOpportunityRepository.GetByIdAsync(id);
            if (entity == null)
                return output.CreateResponse(Constants.MessageCodes.NotFound);
            if(entity.ImprovementSuggestions.Any())
                await _improvementSuggestionsBLL.DeleteByOpportunityId(id);
            _improvementOpportunityRepository.Delete(entity);
            await _unitOfWork.CommitAsync();
            return output.CreateResponse(true);
        }
        public async Task<IResponse<bool>> SaveGeographicalScopeAsync(ImprovementOpportunityGeographicalScope dto, int userId)
        {
            Response<bool> output = new Response<bool>();
            if (dto.Coordinates==null)
                return output.CreateResponse(Constants.MessageCodes.MustSendCoordinates);
            ImprovementOpportunity entity = await _improvementOpportunityRepository.GetByIdAsync(dto.Id);
            if(entity==null)
                return output.CreateResponse(Constants.MessageCodes.NotFound);
            entity.Coordinates = dto.Coordinates;
            await _unitOfWork.CommitAsync();
            return output.CreateResponse(true);
        }
        public async Task<IResponse<string>> getGeographicalScope(int id)
        {
            Response<string> output = new Response<string>();
            ImprovementOpportunity entity = await _improvementOpportunityRepository.GetByIdAsync(id);
            return output.CreateResponse(entity.Coordinates??"");
        }
        //public async Task<IResponse<IList<ImprovementOpportunityGeographicalScopeGetAllDto>>> getAllGeographicalScope(int? UserId)
        //{
        //    Response<IList<ImprovementOpportunityGeographicalScopeGetAllDto>> output = new Response<IList<ImprovementOpportunityGeographicalScopeGetAllDto>>();
        //    List<ImprovementOpportunity> entity = _improvementOpportunityRepository.GetAllList().ToList();
        //    if (entity == null)
        //        return output.CreateResponse(Constants.MessageCodes.NotFound);
        //    return output.CreateResponse(_mapper.Map<IList<ImprovementOpportunityGeographicalScopeGetAllDto>>(entity));
        //}

        public IResponse<PagedResultDto<ImprovementOpportunityGeographicalScopeGetAllDto>> getAllGeographicalScope(ImprovementOpportunityFilteredDto pagedDto, int? userId)
        {
            string? searchWord = pagedDto.SearchTerm?.ToLower().Trim();
            Response<PagedResultDto<ImprovementOpportunityGeographicalScopeGetAllDto>> output = new Response<PagedResultDto<ImprovementOpportunityGeographicalScopeGetAllDto>>();
            PagedResultDto<ImprovementOpportunityGeographicalScopeGetAllDto> list = GetPagedList<ImprovementOpportunityGeographicalScopeGetAllDto, ImprovementOpportunity, int>(pagedDto, repository: _improvementOpportunityRepository, x => x.Id,
                    searchExpression: x =>
                   (pagedDto.Ids == null || pagedDto.Ids != null && pagedDto.Ids.Contains(x.Id)) &&
                   (pagedDto.OpportunityTypeIds == null || pagedDto.OpportunityTypeIds != null && pagedDto.OpportunityTypeIds.Contains(x.OpportunityTypeId.Value)) &&
                   (pagedDto.OpportunityStatusIds == null || pagedDto.OpportunityStatusIds != null && pagedDto.OpportunityStatusIds.Contains(x.OpportunityStatusId.Value)) &&
                   string.IsNullOrEmpty(searchWord) ||
                   (!string.IsNullOrEmpty(searchWord) && x.Name.Contains(searchWord) || x.OpportunityStatus.Name.Contains(searchWord)),
                   sortDirection: "DESC",
              disableFilter: true,
              excluededColumns: null
              );
            return output.CreateResponse(list);
        }
    }
}
