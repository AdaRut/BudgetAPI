using BudgetAPI.DAL.Configurations;
using BudgetAPI.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace BudgetAPI.DAL
{
    public class BudgetDbContext : DbContext
    {
        public BudgetDbContext(DbContextOptions<BudgetDbContext> options) : base(options) { }


        public DbSet<User> Users { get; set; }
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupItem> GroupItems { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.ApplyConfiguration<Budget>(new BudgetConfiguration());
            // TODO modelBuilder.ApplyConfiguration<Group>(new GroupConfiguration());
            // TODO modelBuilder.ApplyConfiguration<GroupItem>(new GroupItemConfiguration());
            modelBuilder.ApplyConfiguration<Role>(new RoleConfiguration());
            modelBuilder.ApplyConfiguration<User>(new UserConfiguration());

            // Seedowanie danych
            // modelBuilder.Entity<Budget>()
            //     .HasData()
        }
    }
}
