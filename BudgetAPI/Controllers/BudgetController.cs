using AutoMapper;
using BudgetAPI.Entities;
using BudgetAPI.Models;
using BudgetAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BudgetAPI.Controllers
{
    [Route("api/budget")]
    [ApiController]
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
        public ActionResult<BudgetDto> Get([FromRoute] int id)
        {
            var budget = this.budgetService.GetById(id);
            return Ok(budget);

        }

        [HttpPost]
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
