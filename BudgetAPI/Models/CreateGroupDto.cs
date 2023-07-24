using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BudgetAPI.Models
{
    public class CreateGroupDto
    {
        [Required]
        public string Name { get; set; }
        public int BudgetId { get; set; }
    }
}
