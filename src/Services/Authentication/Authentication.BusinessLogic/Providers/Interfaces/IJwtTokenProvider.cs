using Authentication.DataLayer.Models;

namespace Authentication.BusinessLogic.Providers.Interfaces;

public interface IJwtTokenProvider
{
    string GenerateToken(User applicationUser, IEnumerable<string> roles);
}