using BackendShop.Core.Dto;
using BackendShop.Core.Interfaces;
using BackendShop.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace BackendShop.BackShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountsService accountsService;

        public AccountsController(IAccountsService accountsService)
        {
            this.accountsService = accountsService;
        }

        [HttpPost("auth/register")]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            await accountsService.Register(model);
            return Ok();
        }

        [HttpPost("auth/login")]
        public async Task<IActionResult> Login(LoginDto model)
        {
            return Ok(await accountsService.Login(model));
        }

        [HttpPost("refreshTokens")]
        public async Task<IActionResult> RefreshTokens(UserTokens tokens)
        {
            return Ok(await accountsService.RefreshTokens(tokens));
        }

        [HttpDelete("logout")]
        public async Task<IActionResult> Logout([FromBody] string refreshToken)
        {
            await accountsService.Logout(refreshToken);
            return Ok();
        }
        //[HttpGet("users")]
        //public async Task<IActionResult> GetAllUsers()
        //{
        //    var users = await accountsService.GetAllUsersAsync();
        //    return Ok(users);
        //}

        //[HttpGet("admins")]
        //public async Task<IActionResult> GetAllAdmins()
        //{
        //    var admins = await accountsService.GetAllAdminsAsync();
        //    return Ok(admins);
        //}




        ///////коментар
        ///плюс коментар
    }
}
