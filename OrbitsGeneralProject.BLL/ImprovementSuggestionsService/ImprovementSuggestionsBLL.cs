using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Orbits.GeneralProject.BLL.BaseReponse;
using Orbits.GeneralProject.BLL.FilesUploaderService;
using Orbits.GeneralProject.BLL.StaticEnums;
using Orbits.GeneralProject.BLL.Validation.ImprovementOpportunityValidation;
using Orbits.GeneralProject.Core.Entities;
using Orbits.GeneralProject.Core.Infrastructure;
using Orbits.GeneralProject.DTO.ImprovementOpportunityDtos;
using Orbits.GeneralProject.DTO.ImprovementSuggestionsDtos;
using Orbits.GeneralProject.DTO.Paging;
using Orbits.GeneralProject.Repositroy.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio.TwiML.Voice;

namespace Orbits.GeneralProject.BLL.ImprovementSuggestionsService
{
    public class ImprovementSuggestionsBLL : BaseBLL, IImprovementSuggestionsBLL
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<ImprovementSuggestion> _improvementSuggestionRepository;
        private readonly IRepository<ImprovementOpportunity> _improvementOpportunity;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<PickFileDraft> _pickedFileRepo;
        private readonly IFileServiceBLL _fileServiceBLL;

        public ImprovementSuggestionsBLL(IMapper mapper,
            IUnitOfWork unitOfWork,
            IRepository<ImprovementSuggestion> improvementSuggestionRepository,
            IRepository<User> userRepository,
            IRepository<PickFileDraft> pickedFileRepo,
            IFileServiceBLL fileServiceBLL,
            IRepository<ImprovementOpportunity> improvementOpportunity
            ) : base(mapper)
        {
            _improvementSuggestionRepository = improvementSuggestionRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _pickedFileRepo = pickedFileRepo;
            _fileServiceBLL = fileServiceBLL;
            _improvementOpportunity = improvementOpportunity;
        }
        public IResponse<PagedResultDto<ImprovementSuggestionViewModel>> GetPagedList(ImprovementSuggestionFilteredDto pagedDto, int userId)
        {
            string? searchWord = pagedDto.SearchTerm?.ToLower().Trim();
            Response<PagedResultDto<ImprovementSuggestionViewModel>> output = new Response<PagedResultDto<ImprovementSuggestionViewModel>>();
            PagedResultDto<ImprovementSuggestionViewModel> list = GetPagedList<ImprovementSuggestionViewModel, ImprovementSuggestion, int>(pagedDto, repository: _improvementSuggestionRepository, x => x.Id,
                searchExpression: x =>
                   (pagedDto.SuggestionStatusIds == null || pagedDto.SuggestionStatusIds != null && pagedDto.SuggestionStatusIds.Contains(x.ImprovementSuggestionStatusId)) &&
                   string.IsNullOrEmpty(searchWord) ||
                   (!string.IsNullOrEmpty(searchWord) && x.Name.Contains(searchWord) || x.ImprovementSuggestionStatus.Name.Contains(searchWord)),
                   sortDirection: "DESC",
              disableFilter: true,
              excluededColumns: null
              );
            return output.CreateResponse(list);
        }
        public async Task<IResponse<bool>> AddAsync(ImprovementSuggestionCreateDto dto, int userId)
        {
            Response<bool> output = new Response<bool>();
            ImprovementSuggestion entity = _mapper.Map<ImprovementSuggestionCreateDto, ImprovementSuggestion>(dto);
            // files required
            entity.FileUploads = _mapper.Map<ICollection<FileUpload>>(dto.Files);
            foreach (var file in dto.Files)
            {
                var entityToRemove = await _pickedFileRepo.GetAsync(x => x.Path == file.FilePath);
                if (entityToRemove != null)
                    _pickedFileRepo.Delete(entityToRemove);
            }
            var opportunity = await _improvementOpportunity.GetByIdAsync(dto.ImprovementOpportunityId);
            if (opportunity == null)
                return output.CreateResponse(Constants.MessageCodes.NotFound);
            entity.ImprovementSuggestionStatusId = (int)ImprovementSuggestionStatusEnum.New;
            entity.CreatedAt = DateTime.Now;
            entity.CreatedByUserId = userId;
            opportunity.OpportunityStatusId = (int)OpportunityStatusEnum.InProgress;
            _improvementSuggestionRepository.Add(entity);
            await _unitOfWork.CommitAsync();
            return output.CreateResponse(true);
        }
        public async Task<IResponse<ImprovementSuggestionGetByIdModel>> GetById(int id, int userId)
        {
            Response<ImprovementSuggestionGetByIdModel> output = new Response<ImprovementSuggestionGetByIdModel>();
            ImprovementSuggestion entity = await _improvementSuggestionRepository.GetByIdAsync(id);
            if (entity == null)
                return output.CreateResponse(Constants.MessageCodes.NotFound);
            return output.CreateResponse(_mapper.Map<ImprovementSuggestionGetByIdModel>(entity));
        }
        public async Task<IResponse<bool>> UpdateAsync(ImprovementSuggestionUpdateDto dto, int userId)
        {
            Response<bool> output = new Response<bool>();
            ImprovementSuggestion entity = await _improvementSuggestionRepository.GetByIdAsync(dto.Id);
            if (entity == null)
                return output.CreateResponse(Constants.MessageCodes.NotFound);
            // files
            await _fileServiceBLL.UpdateFilesAsync(entity.FileUploads, dto.Files);
            entity.Name = dto.Name;
            entity.ModifiedAt = DateTime.Now;
            entity.ModifiedByUserId = userId;
            await _unitOfWork.CommitAsync();
            return output.CreateResponse(true);
        }
        public async Task<IResponse<bool>> Delete(int id, int userId)
        {
            Response<bool> output = new Response<bool>();
            ImprovementSuggestion entity = await _improvementSuggestionRepository.GetByIdAsync(id);
            if (entity == null)
                return output.CreateResponse(Constants.MessageCodes.NotFound);
            //if(entity.ImprovementSuggestionStatusId != (int)ImprovementSuggestionStatusEnum.New)
            //    return output.CreateResponse(Constants.MessageCodes.CanNotDeleteBecauseStatusChanged);
            // remove physical files
            if (entity.FileUploads != null)
                await _fileServiceBLL.DeleteFiles(entity.FileUploads);

            _improvementSuggestionRepository.Delete(entity);
            await _unitOfWork.CommitAsync();
            return output.CreateResponse(true);
        }
        public async Task<bool> DeleteByOpportunityId(int improvementOpportunityId)
        {
            var entities = await _improvementSuggestionRepository.Where(x => x.ImprovementOpportunityId == improvementOpportunityId).ToListAsync();
            if (entities != null)
            {
                var files = entities.SelectMany(x => x.FileUploads).ToList();
                if (files != null)
                    await _fileServiceBLL.DeleteFiles(files);
                _improvementSuggestionRepository.DeleteRange(entities);
                await _unitOfWork.CommitAsync();
            }
            return true;
        }
        public async Task<IResponse<bool>> ChangeStatusOfSuggestion(int id, int statusId, int userId)
        {
            Response<bool> output = new Response<bool>();
            ImprovementSuggestion entity = await _improvementSuggestionRepository.GetByIdAsync(id);
            if (entity == null)
                return output.CreateResponse(Constants.MessageCodes.NotFound);

            if (entity.ImprovementOpportunity.OpportunityStatusId == (int)OpportunityStatusEnum.Approved)
                return output.CreateResponse(Constants.MessageCodes.CanNotAddSuggestionAfterOpportunityApproved);


            if (statusId == (int)ImprovementSuggestionStatusEnum.Certified)
                entity.ImprovementOpportunity.OpportunityStatusId = (int)OpportunityStatusEnum.Approved;

            entity.ImprovementSuggestionStatusId = statusId;
            entity.ModifiedAt = DateTime.Now;
            entity.ModifiedByUserId = userId;
            await _unitOfWork.CommitAsync();
            return output.CreateResponse(true);
        }
    }
}
