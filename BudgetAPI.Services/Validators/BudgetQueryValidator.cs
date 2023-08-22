using BudgetAPI.DAL.Entities;
using BudgetAPI.Services.Models.Budget;
using FluentValidation;
using Microsoft.IdentityModel.Tokens;

namespace BudgetAPI.Services.Validators
{
    public class BudgetQueryValidator : AbstractValidator<BudgetQuery>
    {
        private int[] allowedPagedSizes = new[] { 5, 10, 15 };
        private string[] allowedSortByColumnNames = { nameof(Budget.Name), nameof(Budget.Description) };

        public BudgetQueryValidator()
        {
            RuleFor(r => r.PageNumber).GreaterThanOrEqualTo(1);
            RuleFor(r => r.PageSize).Custom((value, context) =>
            {
                if (!allowedPagedSizes.Contains(value))
                {
                    context.AddFailure("PageSize", $"PageSize must be in [{string.Join(", ", allowedPagedSizes)}].");
                }
            });
            RuleFor(r => r.SortBy).Must(value => string.IsNullOrEmpty(value) || allowedSortByColumnNames.Contains(value))
                .WithMessage($"Sort by is optional, or must be in [{string.Join(", ", allowedSortByColumnNames)}].");
        }
    }
}
