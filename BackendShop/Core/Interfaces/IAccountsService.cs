using BackendShop.Core.Dto;
using BackendShop.Core.Dto.User;
using BackendShop.Data.Entities;

namespace BackendShop.Core.Interfaces
{
    public interface IAccountsService
    {
        Task Register(RegisterDto model);
        Task<UserTokens> Login(LoginDto model);
        Task Logout(string refreshToken);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<IEnumerable<User>> GetAllAdminsAsync();
        Task<UserTokens> RefreshTokens(UserTokens tokens);
        Task RemoveExpiredRefreshTokens();
        Task<User> GetProfileAsync(string userId);
        Task UpdateProfileAsync(string userId, UpdateProfileDto model);
    }
}
