using BudgetAPI.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BudgetAPI.Authorization
{
    public class MinimumAddedRestaurantsRequirementHandler : AuthorizationHandler<MinimumAddedRestaurantsRequirement>
    {
        private readonly BudgetDbContext _budgetDbContext;

        public MinimumAddedRestaurantsRequirementHandler(BudgetDbContext budgetDbContext)
        {
            _budgetDbContext = budgetDbContext;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAddedRestaurantsRequirement requirement)
        {
   
            var userId = int.Parse(context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);

            int addedRestaurants = _budgetDbContext.Budgets.Count(b => b.UserId == userId);

            if (addedRestaurants >= requirement.MinimumRestaurantsAdded)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
