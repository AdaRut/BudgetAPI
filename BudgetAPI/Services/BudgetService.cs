using AutoMapper;
using BudgetAPI.Entities;
using BudgetAPI.Exceptions;
using BudgetAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetAPI.Services
{
    public interface IBudgetService
    {
        BudgetDto GetById(int id);
        IEnumerable<BudgetDto> GetAll();
        int Create(CreateBudgetDto createBudgetDto);
        void Delete(int id);
        void Update(int id, UpdateBudgetDto modifyBudgetDto);

    }

    public class BudgetService : IBudgetService
    {
        private readonly BudgetDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public BudgetService(BudgetDbContext _dbContext, IMapper mapper, ILogger<BudgetService> logger)
        {
            this._dbContext = _dbContext;
            this._mapper = mapper;
            this._logger = logger;

        }

        public BudgetDto GetById(int id)
        {
            var budget = this._dbContext.Budgets
                .Include(r => r.Groupes)
                .ThenInclude(r => r.GroupItems)
                .FirstOrDefault(x => x.Id == id);

            if (budget is null)
                throw new NotFoundException("Budget not found!");

            var result = _mapper.Map<BudgetDto>(budget);
            return result;

        }

        public IEnumerable<BudgetDto> GetAll()
        {
            var budgets = this._dbContext.Budgets
                .Include(r => r.Groupes)
                .ThenInclude(r => r.GroupItems)
                .ToList();

            var budgetsDtos = _mapper.Map<List<BudgetDto>>(budgets);
            return budgetsDtos;
        }

        public int Create(CreateBudgetDto createBudgetDto)
        {
            var budget = _mapper.Map<Budget>(createBudgetDto);
            _dbContext.Budgets.Add(budget);
            _dbContext.SaveChanges();
            return budget.Id;
        }

        public void Delete(int id)
        {
            _logger.LogWarning($"Budget with id: {id} DELETE action Invoked");
            var budget = this._dbContext.Budgets
               .FirstOrDefault(x => x.Id == id);

            if (budget is null)
                throw new NotFoundException("Budget not found!");

            this._dbContext.Budgets.Remove(budget);
            _dbContext.SaveChanges();
        }

        public void Update(int id, UpdateBudgetDto updateBudgetDto)
        {
            var budget = this._dbContext.Budgets
              .FirstOrDefault(x => x.Id == id);

            if (budget is null)
                throw new NotFoundException("Budget not found!");

            budget.Name = updateBudgetDto.Name;
            budget.Description= updateBudgetDto.Description;

            this._dbContext.SaveChanges();
        }
    }
}
