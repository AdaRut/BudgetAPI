using BudgetAPI.Entities;
using System.ComponentModel.DataAnnotations;

namespace BudgetAPI.Models
{
    public class CreateBudgetDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public string Description { get; set; }
        
        [Required]
        public int UserId { get; set;}
    }
}
