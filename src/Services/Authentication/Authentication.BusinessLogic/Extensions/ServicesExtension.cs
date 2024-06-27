using System.Reflection;
using Authentication.BusinessLogic.DTOs.Request;
using Authentication.BusinessLogic.Providers.Implementations;
using Authentication.BusinessLogic.Providers.Interfaces;
using Authentication.BusinessLogic.Services.Implementations;
using Authentication.BusinessLogic.Services.Interfaces;
using Authentication.BusinessLogic.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Authentication.BusinessLogic.Extensions;

public static class ServicesExtension
{
    public static void ConfigureBusinessLogic(this IServiceCollection services)
    {
        services.ConfigureServices();
        services.ConfigureProviders();
    }
    
    private static void ConfigureServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddScoped<IValidator<UserRequestDto>, UserValidator>();
        
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IAuthService, AuthService>();
    }
    
    private static void ConfigureProviders(this IServiceCollection services)
    {
        services.AddScoped<IJwtTokenProvider, JwtTokenProvider>();
        services.AddScoped<IRefreshTokenProvider, RefreshTokenProvider>();
    }
}