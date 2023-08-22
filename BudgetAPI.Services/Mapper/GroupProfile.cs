using AutoMapper;
using BudgetAPI.DAL.Entities;
using BudgetAPI.Services.Models.Group;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetAPI.Services.Mapper
{
    public class GroupProfile : Profile
    {
        public GroupProfile()
        {
            CreateMap<Group, GroupDto>();

            CreateMap<CreateGroupDto, Group>();
        }
    }
}
