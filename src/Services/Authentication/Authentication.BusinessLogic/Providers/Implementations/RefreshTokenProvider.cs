using System.Security.Cryptography;
using Authentication.BusinessLogic.Providers.Interfaces;

namespace Authentication.BusinessLogic.Providers.Implementations;

public class RefreshTokenProvider : IRefreshTokenProvider
{
    public string GenerateRefreshToken()
    {
        byte[] number = new byte[32];
        using (RandomNumberGenerator random = RandomNumberGenerator.Create())
        {
            random.GetBytes(number);
            
            return Convert.ToBase64String(number);
        }
    }
}