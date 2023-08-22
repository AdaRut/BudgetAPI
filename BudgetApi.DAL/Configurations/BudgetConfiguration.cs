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
    internal class BudgetConfiguration : IEntityTypeConfiguration<Budget>
    {
        public void Configure(EntityTypeBuilder<Budget> builder)
        {
            builder
                .Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}
