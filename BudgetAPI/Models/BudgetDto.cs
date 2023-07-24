using BudgetAPI.Entities;

namespace BudgetAPI.Models
{
    public class BudgetDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<GroupDto> Groupes { get; set; } = new List<GroupDto>();
    }
}
