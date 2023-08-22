using BudgetAPI.DAL.Entities;
using BudgetAPI.Services.Models.Group;

namespace BudgetAPI.Services.Models.Budget
{
    public class BudgetDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<GroupDto> Groupes { get; set; } = new List<GroupDto>();
    }
}
