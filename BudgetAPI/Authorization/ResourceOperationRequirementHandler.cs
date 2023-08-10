using BudgetAPI.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BudgetAPI.Authorization
{
    public class ResourceOperationRequirementHandler : AuthorizationHandler<ResourceOperationRequirement, Budget>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceOperationRequirement requirement, Budget budget)
        {
            if(requirement.ResourceOperation == ResourceOperation.Read || 
                requirement.ResourceOperation == ResourceOperation.Create)
            {
                context.Succeed(requirement);
            }
            var userId = context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;
            if(budget.UserId == int.Parse(userId))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
