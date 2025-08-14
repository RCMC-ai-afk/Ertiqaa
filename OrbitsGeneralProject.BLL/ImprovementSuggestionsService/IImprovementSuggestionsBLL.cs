using Orbits.GeneralProject.BLL.BaseReponse;
using Orbits.GeneralProject.DTO.ImprovementSuggestionsDtos;
using Orbits.GeneralProject.DTO.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbits.GeneralProject.BLL.ImprovementSuggestionsService
{
    public interface IImprovementSuggestionsBLL
    {
        IResponse<PagedResultDto<ImprovementSuggestionViewModel>> GetPagedList(ImprovementSuggestionFilteredDto pagedDto, int userId);
        Task<IResponse<bool>> AddAsync(ImprovementSuggestionCreateDto dto, int userId);
        Task<IResponse<ImprovementSuggestionGetByIdModel>> GetById(int id, int userId);
        Task<IResponse<bool>> Delete(int id, int userId);
        Task<bool> DeleteByOpportunityId(int improvementOpportunityId);
        Task<IResponse<bool>> UpdateAsync(ImprovementSuggestionUpdateDto dto, int userId);
        Task<IResponse<bool>> ChangeStatusOfSuggestion(int id, int statusId, int userId);


    }
}
