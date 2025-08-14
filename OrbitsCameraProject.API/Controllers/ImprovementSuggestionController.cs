using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Orbits.GeneralProject.BLL.BaseReponse;
using Orbits.GeneralProject.BLL.ImprovementOpportunityService;
using Orbits.GeneralProject.BLL.ImprovementSuggestionsService;
using Orbits.GeneralProject.BLL.StaticClasses;
using Orbits.GeneralProject.DTO.ImprovementOpportunityDtos;
using Orbits.GeneralProject.DTO.ImprovementSuggestionsDtos;
using Orbits.GeneralProject.DTO.Paging;

namespace OrbitsProject.API.Controllers
{
    [ApiController]
    [Authorize(Roles = UserTypeRoleStatic.AllUserTypes)]
    public class ImprovementSuggestionController : AppBaseController
    {
        private readonly IImprovementSuggestionsBLL _improvementSuggestionsBLL;

        public ImprovementSuggestionController(IImprovementSuggestionsBLL improvementSuggestionsBLL)
        {
            _improvementSuggestionsBLL = improvementSuggestionsBLL;
        }
        [HttpGet, Route("GetResultsByFilter"), ProducesResponseType(typeof(IResponse<PagedResultDto<ImprovementSuggestionViewModel>>), 200)]
        public IActionResult GetResultsByFilter([FromQuery] ImprovementSuggestionFilteredDto paginationFilterModel) =>
            Ok(_improvementSuggestionsBLL.GetPagedList(paginationFilterModel, UserId.Value));

        [HttpPost("AddAsync"), ProducesResponseType(typeof(IResponse<bool>), 200)]
        public async Task<IActionResult> AddAsync(ImprovementSuggestionCreateDto dto) =>
          Ok(await _improvementSuggestionsBLL.AddAsync(dto, UserId.Value));

        [HttpGet("GetById"), ProducesResponseType(typeof(IResponse<ImprovementSuggestionGetByIdModel>), 200)]
        public async Task<IActionResult> GetById(int id) =>
           Ok(await _improvementSuggestionsBLL.GetById(id, UserId.Value));
        [HttpPost("UpdateAsync"), ProducesResponseType(typeof(IResponse<bool>), 200)]
        public async Task<IActionResult> UpdateAsync(ImprovementSuggestionUpdateDto dto) =>
          Ok(await _improvementSuggestionsBLL.UpdateAsync(dto, UserId.Value));
        [HttpPost("Delete"), ProducesResponseType(typeof(IResponse<bool>), 200)]
        public async Task<IActionResult> Delete(int id) =>
            Ok(await _improvementSuggestionsBLL.Delete(id, UserId.Value));

        [HttpPost("ChangeStatusOfSuggestion"), ProducesResponseType(typeof(IResponse<bool>), 200)]
        public async Task<IActionResult> ChangeStatusOfSuggestion(int id,int status) =>
           Ok(await _improvementSuggestionsBLL.ChangeStatusOfSuggestion(id,status, UserId.Value));
    }
}
