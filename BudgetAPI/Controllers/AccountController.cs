using BudgetAPI.Models;
using BudgetAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BudgetAPI.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService= accountService;
        }

        [HttpPost("register")]
        public ActionResult RegisterUser([FromBody] RegisterUserDto dto)
        {
            _accountService.RegisterUserDto(dto);
            return Ok();
        }

        [HttpPost("login")]
        public ActionResult Login([FromBody]LoginDto loginDto)
        {
            String token = _accountService.generateJwt(loginDto);
            return Ok(token);
        }
    }
}
