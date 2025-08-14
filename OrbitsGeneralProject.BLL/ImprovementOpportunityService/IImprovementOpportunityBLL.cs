using Orbits.GeneralProject.BLL.BaseReponse;
using Orbits.GeneralProject.DTO.ImprovementOpportunityDtos;
using Orbits.GeneralProject.DTO.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbits.GeneralProject.BLL.ImprovementOpportunityService
{
    public interface IImprovementOpportunityBLL
    {
        IResponse<PagedResultDto<ImprovementOpportunityViewModel>> GetPagedList(ImprovementOpportunityFilteredDto pagedDto, int userId);
        Task<IResponse<bool>> AddAsync(ImprovementOpportunityCreateDto dto, int userId);
        Task<IResponse<ImprovementOpportunityGetByIdDto>> GetById(int id,int userId);
        Task<IResponse<bool>> UpdateAsync(ImprovementOpportunityUpdateDto dto, int userId);
        Task<IResponse<bool>> DeleteAsync(int id, int userId);
        Task<IResponse<bool>> SaveGeographicalScopeAsync(ImprovementOpportunityGeographicalScope dto, int userId);
        Task<IResponse<string>> getGeographicalScope(int id);
        IResponse<PagedResultDto<ImprovementOpportunityGeographicalScopeGetAllDto>> getAllGeographicalScope(ImprovementOpportunityFilteredDto pagedDto, int? userId);

    }
}
