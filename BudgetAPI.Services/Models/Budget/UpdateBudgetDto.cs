﻿using System.ComponentModel.DataAnnotations;

namespace BudgetAPI.Services.Models.Budget
{
    public class UpdateBudgetDto
    {

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public string Description { get; set; }

    }
}