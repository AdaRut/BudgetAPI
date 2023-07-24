using AutoMapper;
using BudgetAPI.Entities;
using BudgetAPI.Models;

namespace BudgetAPI
{
    public class BudgetMappingProfile : Profile
    {
        public BudgetMappingProfile()
        {
            CreateMap<Budget, BudgetDto>()
                .ForMember(m => m.Groupes, c => c.MapFrom(s => s.Groupes));

            CreateMap<Group, GroupDto>();

            CreateMap<GroupItem, GroupItemDto>();

            CreateMap<CreateBudgetDto, Budget>();

            CreateMap<CreateGroupDto, Group>();
        }
    }
}
