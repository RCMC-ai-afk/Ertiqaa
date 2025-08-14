using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Orbits.GeneralProject.BLL.BaseReponse;
using Orbits.GeneralProject.BLL.ImprovementOpportunityService;
using Orbits.GeneralProject.BLL.StaticClasses;
using Orbits.GeneralProject.DTO.ImprovementOpportunityDtos;
using Orbits.GeneralProject.DTO.Paging;
using Orbits.GeneralProject.DTO.TempIncidentDTOs;

namespace OrbitsProject.API.Controllers
{
    [ApiController]
    [Authorize]

    public class ImprovementOpportunityController : AppBaseController
    {
        private readonly IImprovementOpportunityBLL _improvementOpportunityService;
        public ImprovementOpportunityController(IImprovementOpportunityBLL improvementOpportunityService)
        {
            _improvementOpportunityService = improvementOpportunityService;
        }

        [HttpGet, Route("GetResultsByFilter"), ProducesResponseType(typeof(IResponse<PagedResultDto<ImprovementOpportunityViewModel>>), 200)]
        public IActionResult GetResultsByFilter([FromQuery] ImprovementOpportunityFilteredDto paginationFilterModel)=>
            Ok(_improvementOpportunityService.GetPagedList(paginationFilterModel, UserId.Value));

        [HttpPost("AddAsync"), ProducesResponseType(typeof(IResponse<bool>), 200)]
        public async Task<IActionResult> AddAsync(ImprovementOpportunityCreateDto dto) =>
          Ok(await _improvementOpportunityService.AddAsync(dto, UserId.Value));

        [HttpGet("GetById"), ProducesResponseType(typeof(IResponse<ImprovementOpportunityGetByIdDto>), 200)]
        public async Task<IActionResult> GetById(int id) =>
           Ok(await _improvementOpportunityService.GetById(id, UserId.Value));

        [HttpPost("UpdateAsync"), ProducesResponseType(typeof(IResponse<bool>), 200)]
        public async Task<IActionResult> UpdateAsync(ImprovementOpportunityUpdateDto dto) =>
        Ok(await _improvementOpportunityService.UpdateAsync(dto, UserId.Value));

        [HttpPost("Delete"), ProducesResponseType(typeof(IResponse<bool>), 200)]
        public async Task<IActionResult> Delete(int id) =>
            Ok(await _improvementOpportunityService.DeleteAsync(id, UserId.Value));

        [HttpPost("SaveGeographicalScopeAsync"), ProducesResponseType(typeof(IResponse<bool>), 200)]
        public async Task<IActionResult> SaveGeographicalScopeAsync(ImprovementOpportunityGeographicalScope dto) =>
          Ok(await _improvementOpportunityService.SaveGeographicalScopeAsync(dto, UserId.Value));

        [HttpGet("getGeographicalScope"), ProducesResponseType(typeof(IResponse<string>), 200)]
        public async Task<IActionResult> getGeographicalScope(int id) =>
         Ok(await _improvementOpportunityService.getGeographicalScope(id));
       
        [HttpGet, Route("getAllGeographicalScope"), ProducesResponseType(typeof(IResponse<PagedResultDto<ImprovementOpportunityViewModel>>), 200)]
        public IActionResult getAllGeographicalScope([FromQuery] ImprovementOpportunityFilteredDto paginationFilterModel) =>
           Ok(_improvementOpportunityService.getAllGeographicalScope(paginationFilterModel, this.UserId));
    }
}
