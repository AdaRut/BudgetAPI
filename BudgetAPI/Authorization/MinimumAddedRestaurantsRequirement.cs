using Microsoft.AspNetCore.Authorization;

namespace BudgetAPI.Authorization
{
    public class MinimumAddedRestaurantsRequirement : IAuthorizationRequirement
    {
        public int MinimumRestaurantsAdded { get; }
        public MinimumAddedRestaurantsRequirement(int minimumRestaurantsAdded)
        {
            this.MinimumRestaurantsAdded = minimumRestaurantsAdded;
        }
    }
}
