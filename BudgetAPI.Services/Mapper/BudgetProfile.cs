using AutoMapper;
using BudgetAPI.DAL.Entities;
using BudgetAPI.Services.Models.Budget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetAPI.Services.Mapper
{
    public class BudgetProfile : Profile
    {
        public BudgetProfile() 
        {
            CreateMap<Budget, BudgetDto>()
            .ForMember(m => m.Groupes, c => c.MapFrom(s => s.Groupes));

            CreateMap<CreateBudgetDto, Budget>();
        }
    }
}
