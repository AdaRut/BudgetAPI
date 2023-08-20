using AutoMapper;
using BudgetAPI.Authorization;
using BudgetAPI.DAL;
using BudgetAPI.DAL.Entities;
using BudgetAPI.Exceptions;
using BudgetAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Security.Claims;

namespace BudgetAPI.Services
{
    public interface IBudgetService
    {
        BudgetDto GetById(int id);
        PagedResult<BudgetDto> GetAll(BudgetQuery budgetQuery);
        int Create(CreateBudgetDto createBudgetDto);
        void Delete(int id);
        void Update(int id, UpdateBudgetDto modifyBudgetDto);

    }

    public class BudgetService : IBudgetService
    {
        private readonly BudgetDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserContextService _userContextService;

        public BudgetService(
            BudgetDbContext _dbContext,
            IMapper mapper,
            ILogger<BudgetService> logger,
            IAuthorizationService authorizationService,
            IUserContextService userContextService)
        {
            this._dbContext = _dbContext;
            this._mapper = mapper;
            this._logger = logger;
            this._authorizationService = authorizationService;
            this._userContextService = userContextService;
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

        public PagedResult<BudgetDto> GetAll(BudgetQuery budgetQuery)
        {
            var baseQuery = this._dbContext.Budgets
                .Include(r => r.Groupes)
                .ThenInclude(r => r.GroupItems)
                .Where(r => budgetQuery.SearchPhrase == null ||
                (r.Name.ToLower().Contains(budgetQuery.SearchPhrase.ToLower())
                    || r.Description.ToLower().Contains(budgetQuery.SearchPhrase.ToLower())));

            if(!string.IsNullOrEmpty(budgetQuery.SortBy))
            {
                var columnsSelector = new Dictionary<string, Expression<Func<Budget, object>>>()
                {
                    { nameof(Budget.Name), r => r.Name },
                    { nameof(Budget.Description), r => r.Description },
                };

                var selectedColumn = columnsSelector[budgetQuery.SortBy];

                baseQuery = budgetQuery.SortDirection == SortDirection.ASC 
                    ? baseQuery.OrderBy(selectedColumn)
                    : baseQuery.OrderByDescending(selectedColumn);
            }

            var budgets = baseQuery
                .Skip((budgetQuery.PageNumber - 1) * budgetQuery.PageSize)
                .Take(budgetQuery.PageSize)
                .ToList();
                

            var budgetsDtos = _mapper.Map<List<BudgetDto>>(budgets);

            var pagedResult = new PagedResult<BudgetDto>(budgetsDtos, baseQuery.Count(), budgetQuery.PageSize, budgetQuery.PageNumber);
            return pagedResult;
        }

        public int Create(CreateBudgetDto createBudgetDto)
        {
            var budget = _mapper.Map<Budget>(createBudgetDto);
            budget.UserId = (int)_userContextService.GetUserId;
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

            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, budget, new ResourceOperationRequirement(ResourceOperation.Delete)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbiddenException("You can't delete this budget!");
            }


            this._dbContext.Budgets.Remove(budget);
            _dbContext.SaveChanges();
        }

        public void Update(int id, UpdateBudgetDto updateBudgetDto)
        {
            var budget = this._dbContext.Budgets
              .FirstOrDefault(x => x.Id == id);

            if (budget is null)
                throw new NotFoundException("Budget not found!");

            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, budget, new ResourceOperationRequirement(ResourceOperation.Update)).Result;

            if(!authorizationResult.Succeeded)
            {
                throw new ForbiddenException("You can't update this budget!");
            }

            budget.Name = updateBudgetDto.Name;
            budget.Description= updateBudgetDto.Description;

            this._dbContext.SaveChanges();
        }
    }
}
