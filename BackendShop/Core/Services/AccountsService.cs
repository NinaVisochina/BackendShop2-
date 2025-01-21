using AutoMapper;
using BackendShop.Core.Dto;
using BackendShop.Core.Exceptions;
using BackendShop.Core.Interfaces;
using BackendShop.Data.Entities;
using BackendShop.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BackendShop.Core.Services
{
    public class AccountsService(
        UserManager<User> userManager,
        IMapper mapper,
        IJwtService jwtService,
        IRepository<RefreshToken> refreshTokenR) : IAccountsService
    {
        private readonly UserManager<User> userManager = userManager;
        private readonly IMapper mapper = mapper;
        private readonly IJwtService jwtService = jwtService;
        private readonly IRepository<RefreshToken> refreshTokenR = refreshTokenR;

        public async Task Register(RegisterDto model)
        {
            //var user = new User()
            //{
            //    Email = model.Email,
            //    UserName = model.Email,
            //    Birthdate = model.Birthdate,
            //    PhoneNumber = model.PhoneNumber
            //};
            var user = mapper.Map<User>(model);

            var result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                //string all = string.Join(" ", result.Errors.Select(x => x.Description));
                var error = result.Errors.First();
                throw new HttpException(error.Description, HttpStatusCode.BadRequest);
            }
            // Додати користувача до ролі "User"
            await userManager.AddToRoleAsync(user, "User");
        }

        public async Task<UserTokens> Login(LoginDto model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);

            if (user == null || !await userManager.CheckPasswordAsync(user, model.Password))
                throw new HttpException("Invalid login or password.", HttpStatusCode.BadRequest);

            // generate access token... (JWT)
            return new UserTokens
            {
                AccessToken = jwtService.CreateToken(jwtService.GetClaims(user)),
                RefreshToken = CreateRefreshToken(user.Id).Token
            };
        }

        public async Task Logout(string refreshToken)
        {
            var record = (await refreshTokenR.Get(x => x.Token == refreshToken)).FirstOrDefault();
            if (record == null) return;

            await refreshTokenR.Delete(record);
            await refreshTokenR.Save();
        }

        private RefreshToken CreateRefreshToken(string userId)
        {
            var refeshToken = jwtService.CreateRefreshToken();

            var refreshTokenEntity = new RefreshToken
            {
                Token = refeshToken,
                UserId = userId,
                CreationDate = DateTime.UtcNow // Now vs UtcNow
            };

            refreshTokenR.Insert(refreshTokenEntity);
            refreshTokenR.Save();

            return refreshTokenEntity;
        }

        public async Task<UserTokens> RefreshTokens(UserTokens userTokens)
        {
            var refrestToken = (await refreshTokenR.Get(x => x.Token == userTokens.RefreshToken)).FirstOrDefault();

            if (refrestToken == null || jwtService.IsRefreshTokenExpired(refrestToken.CreationDate))
                throw new HttpException("Invalid token.", HttpStatusCode.BadRequest);

            var claims = jwtService.GetClaimsFromExpiredToken(userTokens.AccessToken);
            var newAccessToken = jwtService.CreateToken(claims);
            var newRefreshToken = jwtService.CreateRefreshToken();

            // update refresh token in db
            refrestToken.Token = newRefreshToken;
            refrestToken.CreationDate = DateTime.UtcNow;

            await refreshTokenR.Update(refrestToken);
            await refreshTokenR.Save();

            var tokens = new UserTokens()
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };

            return tokens;
        }

        public async Task RemoveExpiredRefreshTokens()
        {
            var lastDate = jwtService.GetLastValidRefreshTokenDate();
            var expiredTokens = await refreshTokenR.Get(x => x.CreationDate < lastDate);

            foreach (var i in expiredTokens)
            {
                await refreshTokenR.Delete(i);
            }
            await refreshTokenR.Save();
        }

        //public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        //{
        //    var users = await userManager.GetUsersInRoleAsync("User");
        //    return users.Select(user => new UserDto
        //    {
        //        Id = user.UserId,
        //        Firstname = user.Firstname,
        //        Lastname = user.Lastname,
        //        Email = user.Email
        //    });
        //}

        //public async Task<IEnumerable<UserDto>> GetAllAdminsAsync()
        //{
        //    var admins = await userManager.GetUsersInRoleAsync("Admin");
        //    return admins.Select(admin => new UserDto
        //    {
        //        Id = admin.UserId,
        //        Firstname = admin.Firstname,
        //        Lastname = admin.Lastname,
        //        Email = admin.Email
        //    });
        //}
    }
}
