using BudgetAPI.Models;
using System.Security.Principal;

namespace BudgetAPI.Services.Models.Budget
{
    public class BudgetQuery
    {
        public string? SearchPhrase { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? SortBy { get; set; }
        public SortDirection? SortDirection { get; set; }
    }
}
