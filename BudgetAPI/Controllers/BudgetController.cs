using AutoMapper;
using BudgetAPI.Entities;
using BudgetAPI.Models;
using BudgetAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BudgetAPI.Controllers
{
    [Route("api/budget")]
    [ApiController]
    [Authorize]
    public class BudgetController : ControllerBase
    {
        private readonly IBudgetService budgetService;
        public BudgetController(IBudgetService budgetService)
        {
            this.budgetService = budgetService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<BudgetDto>> GetAll()
        {
            
            var budgetsDtos = budgetService.GetAll();
            return Ok(budgetsDtos);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public ActionResult<BudgetDto> Get([FromRoute] int id)
        {
            var budget = this.budgetService.GetById(id);
            return Ok(budget);

        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Authorize(Policy = "HasUsernameADMIN123")]
        public ActionResult CreateBudget([FromBody]CreateBudgetDto budgetDto)
        {
            var id = this.budgetService.Create(budgetDto);
            return Created($"/api/budget/{id}", null);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id) 
        { 
            this.budgetService.Delete(id);
            return NoContent();
           
        }

        [HttpPut("{id}")]
        public ActionResult Update([FromRoute] int id, [FromBody]UpdateBudgetDto budgetDto)
        {
            this.budgetService.Update(id, budgetDto);
            return Ok();
        }
    }
}
