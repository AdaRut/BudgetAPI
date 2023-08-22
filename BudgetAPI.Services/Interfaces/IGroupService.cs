using BudgetAPI.Services.Models.Group;

namespace BudgetAPI.Services.Interfaces
{
    public interface IGroupService
    {
        int Create(int id, CreateGroupDto dto);

        GroupDto GetById(int budgetId, int groupId);

        List<GroupDto> GetAll(int budgetId);
        void RemoveAll(int budgetId);
    }
}
