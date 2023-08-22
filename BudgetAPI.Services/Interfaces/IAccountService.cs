using BudgetAPI.Services.Models.User;

namespace BudgetAPI.Services.Interfaces
{
    public interface IAccountService
    {
        void RegisterUserDto(RegisterUserDto dto);
        string generateJwt(LoginDto dto);
    }
}
