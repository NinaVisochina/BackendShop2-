using BackendShop.Data.Entities;
using System.Security.Claims;

namespace BackendShop.Core.Interfaces
{
    public interface IJwtService
    {
        // ------- Access Token
        IEnumerable<Claim> GetClaims(User user);
        string CreateToken(IEnumerable<Claim> claims);

        // ------- Refresh Token
        string CreateRefreshToken();
        IEnumerable<Claim> GetClaimsFromExpiredToken(string token);
        bool IsRefreshTokenExpired(DateTime creationTime);
        DateTime GetLastValidRefreshTokenDate();
    }
}
