using BackendShop.Core.Dto;
using BackendShop.Core.Dto.User;
using BackendShop.Core.Exceptions;
using BackendShop.Core.Interfaces;
using BackendShop.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Security.Claims;

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

        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized();

            var userProfile = await accountsService.GetProfileAsync(userId);
            return Ok(userProfile);
        }

        [HttpPut("profile")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized();

            await accountsService.UpdateProfileAsync(userId, model);
            return Ok(new { message = "Профіль успішно оновлено" });
        }

        [HttpGet("users")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await accountsService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("admins")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllAdmins()
        {
            var admins = await accountsService.GetAllAdminsAsync();
            return Ok(admins);
        }




    }
}
