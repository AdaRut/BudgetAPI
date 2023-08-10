using BudgetAPI.Entities;
using BudgetAPI.Exceptions;
using BudgetAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BudgetAPI.Services
{
    public interface IAccountService
    {
        void RegisterUserDto(RegisterUserDto dto);
        String generateJwt(LoginDto dto);
    }
    public class AccountService : IAccountService
    {
        private readonly BudgetDbContext _dbContext;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly AuthenticationSettings _authenticationSettings;

        public AccountService(BudgetDbContext dbContext, IPasswordHasher<User> passwordHasher, AuthenticationSettings authenticationSettings)
        {
            this._dbContext = dbContext;
            this._passwordHasher = passwordHasher;
            this._authenticationSettings = authenticationSettings;
        }

        public string generateJwt(LoginDto dto)
        {
            var user = _dbContext.Users.Include(user => user.Role).FirstOrDefault(x => x.Email== dto.Email);
            if(user==null)
            {
                throw new BadRequestException("Invalid username or password!");
            }
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if(result == PasswordVerificationResult.Failed)
            {
                throw new BadRequestException("Invalid username or password!");
            }
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Role, user.Role.Name),
                new Claim("DateOfBirth", user.BirthdayDate.Value.ToString("yyyy-MM-dd"))
            };

            if(!string.IsNullOrEmpty(user.UserName))
            {
                claims.Add(new Claim("Username", user.UserName));
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiryDate = DateTime.Now.AddHours(_authenticationSettings.JwtExpireHours);

            var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer,
                _authenticationSettings.JwtIssuer,
                claims,
                expires: expiryDate,
                signingCredentials: credentials);

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);  
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
