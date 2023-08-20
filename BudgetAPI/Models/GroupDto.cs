using BudgetAPI.DAL.Entities;

namespace BudgetAPI.Models
{
    public class GroupDto
    {
        public string Name { get; set; }
        public List<GroupItemDto> GroupItems { get; set; } = new List<GroupItemDto>();
    }
}
