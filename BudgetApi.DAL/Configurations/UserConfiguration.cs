using BudgetAPI.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace BudgetAPI.DAL.Configurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .Property(e => e.Id)
                .IsRequired();
            builder
                .Property(e => e.Email)
                .IsRequired();
        }
    }
}
