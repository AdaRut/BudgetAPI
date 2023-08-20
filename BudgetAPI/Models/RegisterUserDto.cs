using BudgetAPI.DAL;
using System.ComponentModel.DataAnnotations;

namespace BudgetAPI.Models
{
    public class RegisterUserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        public DateTime? BirthdayDate { get; set; }
        [Required]
        [MinLength(8)]
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string UserName { get; set; }
        public int RoleId { get; set; } = 1;
    }
}
