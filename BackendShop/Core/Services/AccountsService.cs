using AutoMapper;
using BackendShop.Core.Dto;
using BackendShop.Core.Dto.User;
using BackendShop.Core.Exceptions;
using BackendShop.Core.Interfaces;
using BackendShop.Data.Entities;
using BackendShop.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Security.Claims;
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
            var entity = await CreateRefreshToken(user.Id);
            return new UserTokens
            {
                AccessToken = jwtService.CreateToken(jwtService.GetClaims(user)),
                RefreshToken = entity.Token,
                UserId = user.Id // Додаємо userId до відповіді
            };
        }

        public async Task Logout(string refreshToken)
        {
            var record = (await refreshTokenR.Get(x => x.Token == refreshToken)).FirstOrDefault();
            if (record == null) return;

            await refreshTokenR.Delete(record);
            await refreshTokenR.Save();
        }

        private async Task<RefreshToken> CreateRefreshToken(string userId)
        {
            var refeshToken = jwtService.CreateRefreshToken();

            var refreshTokenEntity = new RefreshToken
            {
                Token = refeshToken,
                UserId = userId,
                CreationDate = DateTime.UtcNow // Now vs UtcNow
            };

            await refreshTokenR.Insert(refreshTokenEntity);
            await refreshTokenR.Save();

            return refreshTokenEntity;
        }

        public async Task<UserTokens> RefreshTokens(UserTokens userTokens)
        {
            //// Розпарсити claims із протермінованого токена
            //var claims = jwtService.GetClaimsFromExpiredToken(userTokens.AccessToken);

            //// Знайти користувача через claims
            //var userId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            //if (string.IsNullOrEmpty(userId))
            //    throw new HttpException("Invalid access token.", HttpStatusCode.BadRequest);

            //var user = await userManager.FindByIdAsync(userId);

            //if (user == null)
            //    throw new HttpException("User not found.", HttpStatusCode.NotFound);

            //// Перевірити рефреш-токен
            //var refreshToken = user.RefreshTokens.FirstOrDefault(rt => rt.Token == userTokens.RefreshToken);

            //if (refreshToken == null || jwtService.IsRefreshTokenExpired(refreshToken.CreationDate))
            //    throw new HttpException("Invalid or expired refresh token.", HttpStatusCode.Unauthorized);

            //// Оновити рефреш-токен (якщо потрібно)
            //var newRefreshToken = jwtService.CreateRefreshToken();
            //refreshToken.Token = newRefreshToken;
            //refreshToken.CreationDate = DateTime.UtcNow;

            //await userManager.UpdateAsync(user);

            //// Створити новий access токен
            //var newAccessToken = jwtService.CreateToken(claims);

            //return new UserTokens
            //{
            //    AccessToken = newAccessToken,
            //    RefreshToken = newRefreshToken
            //};

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
        public async Task<User> GetProfileAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
                throw new HttpException("User not found.", HttpStatusCode.NotFound);

            return new User
            {
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Birthdate = user.Birthdate
            };
        }

        public async Task UpdateProfileAsync(string userId, UpdateProfileDto model)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
                throw new Exception("Користувача не знайдено");

            // Оновлюємо дані користувача
            user.Firstname = model.FirstName;
            user.Lastname = model.LastName;
            user.PhoneNumber = model.PhoneNumber;
            user.Email = model.Email;

            // Додайте валідацію для інших полів, якщо потрібно
            if (!string.IsNullOrEmpty(model.Birthdate))
            {
                if (DateTime.TryParse(model.Birthdate, out var birthdate))
                {
                    user.Birthdate = birthdate;
                }
                else
                {
                    throw new Exception("Некоректний формат дати народження");
                }
            }

            // Зберігаємо зміни
            var result = await userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            }
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
