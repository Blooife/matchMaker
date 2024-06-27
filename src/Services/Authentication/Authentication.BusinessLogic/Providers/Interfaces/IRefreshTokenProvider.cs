namespace Authentication.BusinessLogic.Providers.Interfaces;

public interface IRefreshTokenProvider
{
    string GenerateRefreshToken();
}