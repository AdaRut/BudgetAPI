using System.ComponentModel.DataAnnotations;

namespace BudgetAPI.Services.Models.Group
{
    public class CreateGroupDto
    {
        [Required]
        public string Name { get; set; }
        public int BudgetId { get; set; }
    }
}
