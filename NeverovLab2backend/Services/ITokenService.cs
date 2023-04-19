using NeverovLab2backend.Data;
using System.Security.Claims;

namespace NeverovLab2backend.Services
{
    public interface ITokenService
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        bool CheckToken(User user);
        bool CheckTime(User user);
    }
}
