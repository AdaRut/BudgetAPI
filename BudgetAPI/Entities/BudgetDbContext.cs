using Microsoft.EntityFrameworkCore;

namespace BudgetAPI.Entities
{
    public class BudgetDbContext : DbContext
    {
        private string _connectionString =
            "Server=(localdb)\\mssqllocaldb;Database=BudgetDb;Trusted_Connection=true;";
        public DbSet<User> Users { get; set; }
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupItem> GroupItems { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(e => e.Id)
                .IsRequired();
            modelBuilder.Entity<User>()
                .Property(e => e.Email)
                .IsRequired();

            modelBuilder.Entity<Role>()
                .Property(e => e.Name)
                .IsRequired();

            modelBuilder.Entity<Budget>()
                .Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);

           // Seedowanie danych
           // modelBuilder.Entity<Budget>()
           //     .HasData()
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
            base.OnConfiguring(optionsBuilder);
        }
    }
}
