using AutoMapper;
using BudgetAPI.DAL;
using BudgetAPI.DAL.Entities;
using BudgetAPI.Exceptions;
using BudgetAPI.Services.Interfaces;
using BudgetAPI.Services.Models.Group;
using Microsoft.EntityFrameworkCore;

namespace BudgetAPI.Services
{

    public class GroupService : IGroupService
    {
        private readonly BudgetDbContext _context;
        private readonly IMapper _mapper;

        public GroupService(BudgetDbContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public int Create(int id, CreateGroupDto dto)
        {
            var budget = GetBudgetById(id);
            var groupEntity = _mapper.Map<Group>(dto);
            groupEntity.BudgetId = id;
            _context.Groups.Add(groupEntity);
            _context.SaveChanges();
            return groupEntity.Id;
        }

        public GroupDto GetById(int budgetId, int groupId)
        {
            var budget = GetBudgetById(budgetId);
            var group = _context.Groups
                .Include(x => x.GroupItems)
                .FirstOrDefault(x => x.Id == groupId);
                
            if(group == null || group.BudgetId != budgetId)
            {
                throw new NotFoundException("Group not found!");
            }
            var groupDto = _mapper.Map<GroupDto>(group);
            return groupDto;
        }

        public List<GroupDto> GetAll(int budgetId)
        {
            var budget = GetBudgetById(budgetId);
            var groupDtos = _mapper.Map<List<GroupDto>>(budget.Groupes);
            return groupDtos;
        }

        public void RemoveAll(int budgetId)
        {
            var budget = GetBudgetById(budgetId);
            _context.RemoveRange(budget.Groupes);
            _context.SaveChanges();

        }

        private Budget GetBudgetById(int id)
        {
            var budget = this._context.Budgets
                .Include(x => x.Groupes)
                .ThenInclude(x => x.GroupItems)
                .FirstOrDefault(x => x.Id == id);
            if (budget == null)
            {
                throw new NotFoundException("Budget not found!");
            }
            return budget;
        }
    }
}
