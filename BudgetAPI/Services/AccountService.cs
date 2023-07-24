using BudgetAPI.Entities;
using BudgetAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace BudgetAPI.Services
{
    public interface IAccountService
    {
        void RegisterUserDto(RegisterUserDto dto);
    }
    public class AccountService : IAccountService
    {
        private readonly BudgetDbContext _dbContext;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AccountService(BudgetDbContext dbContext, IPasswordHasher<User> passwordHasher)
        {
            this._dbContext = dbContext;
            this._passwordHasher = passwordHasher;
        }
        public void RegisterUserDto(RegisterUserDto dto)
        {
            var newUser = new User()
            {
                UserName = dto.UserName,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                BirthdayDate = dto.BirthdayDate,
                RoleId = dto.RoleId,
            };
            var hashedPassword = _passwordHasher.HashPassword(newUser, dto.Password);
            newUser.PasswordHash = hashedPassword;
            _dbContext.Users.Add(newUser);
            _dbContext.SaveChanges();
        }
    }
}
