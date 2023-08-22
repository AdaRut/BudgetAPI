using BudgetAPI.Models;
using BudgetAPI.Services;
using BudgetAPI.Services.Interfaces;
using BudgetAPI.Services.Models.Group;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BudgetAPI.Controllers
{
    [Route("api/budget/{budgetId}/group")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _groupService;
        public GroupController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        [HttpPost]
        public ActionResult Post([FromRoute] int budgetId, [FromBody] CreateGroupDto dto)
        {
            var id = this._groupService.Create(budgetId, dto);
            return Created($"/api/budget/{budgetId}/group/{id}", null);

        }

        [HttpGet("{groupId}")]
        public ActionResult<GroupDto> Get([FromRoute] int budgetId, [FromRoute] int groupId)
        {
            var group = _groupService.GetById(budgetId, groupId);
            return Ok(group);
        }

        [HttpGet]
        public ActionResult<List<GroupDto>> GetAll([FromRoute] int budgetId)
        {
            var groupDtos = _groupService.GetAll(budgetId);
            return Ok(groupDtos);
        }

        [HttpDelete]
        public ActionResult Delete([FromRoute] int budgetId)
        {
            _groupService.RemoveAll(budgetId);
            return NoContent();
        }
    }
}
