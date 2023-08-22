using AutoMapper;
using BudgetAPI.DAL.Entities;
using BudgetAPI.Services.Models.GroupItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetAPI.Services.Mapper
{
    public class GroupItemProfile : Profile
    {
        public GroupItemProfile()
        {
            CreateMap<GroupItem, GroupItemDto>();
        }
    }
}
