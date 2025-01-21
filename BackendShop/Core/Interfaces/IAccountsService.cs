using BackendShop.Core.Dto;

namespace BackendShop.Core.Interfaces
{
    public interface IAccountsService
    {
        Task Register(RegisterDto model);
        Task<UserTokens> Login(LoginDto model);
        Task Logout(string refreshToken);
        //Task<IEnumerable<UserDto>> GetAllUsersAsync();
        //Task<IEnumerable<UserDto>> GetAllAdminsAsync();
        Task<UserTokens> RefreshTokens(UserTokens tokens);
        Task RemoveExpiredRefreshTokens();
    }
}
