using BudgetAPI.Models;
using BudgetAPI.Services.Models.Budget;

namespace BudgetAPI.Services.Interfaces
{
    public interface IBudgetService
    {
        BudgetDto GetById(int id);
        PagedResult<BudgetDto> GetAll(BudgetQuery budgetQuery);
        int Create(CreateBudgetDto createBudgetDto);
        void Delete(int id);
        void Update(int id, UpdateBudgetDto modifyBudgetDto);
    }
}
