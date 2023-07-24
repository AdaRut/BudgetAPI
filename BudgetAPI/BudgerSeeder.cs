using BudgetAPI.Entities;

namespace BudgetAPI
{
    public class BudgerSeeder
    {
        private BudgetDbContext budgetDbContext;

        public BudgerSeeder(BudgetDbContext budgetDbContext)
        {
            this.budgetDbContext = budgetDbContext;
        }

        public void SeedData()
        {
            if (budgetDbContext.Database.CanConnect())
            {
                if (!budgetDbContext.Roles.Any())
                {
                    var roles = GetRoles();
                    budgetDbContext.Roles.AddRange(roles);
                    budgetDbContext.SaveChanges();
                }
                if (!budgetDbContext.Budgets.Any())
                {
                    var budgets = GetBudgets();
                    budgetDbContext.Budgets.AddRange(budgets);
                    budgetDbContext.SaveChanges();
                }

                
            }
        }

        private IEnumerable<Budget> GetBudgets()
        {
            var Budgets = new List<Budget>()
            {
                new Budget()
                {
                    User = new User()
                    {
                        FirstName = "ADMIN",
                        UserName = "ADMIN",
                        LastName= "ADMIN",
                        PasswordHash= "ADMIN",
                        Email="ADMIN123@gmail.com",
                        RoleId=2
                    },
                    Name= "Lipiec 2023",
                    Description="Budżet na lipiec 2023",
                    Groupes = new List<Group>()
                    {
                        new Group()
                        {
                            Name="Niezbedne do przezycia",
                            GroupItems=new List<GroupItem>()
                            {
                                new GroupItem()
                                {
                                    Name="Jedzenie i picie",
                                    PlannedAmount=1600,
                                    SpendAmount=0,
                                    Notes="",
                                    IsPaid=false,

                                },
                                new GroupItem()
                                {
                                    Name="Czynsz",
                                    PlannedAmount=948.50m,
                                    SpendAmount=0,
                                    Notes="",
                                    IsPaid=false,

                                }
                            }
                        },
                        new Group()
                        {
                            Name="Raty, kredyty, pożyczki",
                            GroupItems=new List<GroupItem>()
                            {
                                new GroupItem()
                                {
                                    Name="Millenium Rata",
                                    PlannedAmount=327,
                                    SpendAmount=0,
                                    Notes = "",
                                    IsPaid = false,

                                },
                                new GroupItem()
                                {
                                    Name="Auto",
                                    PlannedAmount=2000m,
                                    SpendAmount=0,
                                    Notes = "",
                                    IsPaid = false,

                                }
                            }
                        }
                    }
                }
            };
            return Budgets;
        }

        private IEnumerable<Role> GetRoles()
        {
            var Roles = new List<Role>()
            {
                new Role()
                {
                    Name= "User"
                },
                new Role()
                {
                    Name="Admin"
                }
            };
            return Roles;
        }
    }
}
