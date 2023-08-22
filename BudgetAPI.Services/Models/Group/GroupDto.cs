using BudgetAPI.DAL.Entities;
using BudgetAPI.Services.Models.GroupItem;

namespace BudgetAPI.Services.Models.Group
{
    public class GroupDto
    {
        public string Name { get; set; }
        public List<GroupItemDto> GroupItems { get; set; } = new List<GroupItemDto>();
    }
}
