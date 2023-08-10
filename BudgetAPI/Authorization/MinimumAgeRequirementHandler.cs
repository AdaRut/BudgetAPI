using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BudgetAPI.Authorization
{
    public class MinimumAgeRequirementHandler : AuthorizationHandler<MinimumAgeRequirement>
    {
        private readonly ILogger<MinimumAgeRequirementHandler> _logger;

        public MinimumAgeRequirementHandler(ILogger<MinimumAgeRequirementHandler> logger)
        {
            _logger = logger;
        }

     

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAgeRequirement requirement)
        {
            var dateOfBirth =DateTime.Parse(context.User.FindFirst(c => c.Type == "DateOfBirth").Value);

            var username = context.User.FindFirst(c => c.Type == ClaimTypes.Name).Value;

            _logger.LogInformation($"{username} with date of Birth: {dateOfBirth}");

            if (dateOfBirth.AddYears(requirement.MinimumAge) <= DateTime.Today)
            {
                _logger.LogInformation("Authorization succeed");
                context.Succeed(requirement);
            } else
            {
                _logger.LogInformation("Authorization failed");
            }
            return Task.CompletedTask;
        }
    }
}
