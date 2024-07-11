using System.Reflection;
using Authentication.BusinessLogic.DTOs.Request;
using Authentication.BusinessLogic.Hangfire;
using Authentication.BusinessLogic.Providers.Implementations;
using Authentication.BusinessLogic.Providers.Interfaces;
using Authentication.BusinessLogic.Services.Implementations;
using Authentication.BusinessLogic.Services.Interfaces;
using Authentication.BusinessLogic.Validators;
using FluentValidation;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Authentication.BusinessLogic.Extensions;

public static class ServicesExtension
{
    public static void ConfigureBusinessLogic(this IServiceCollection services, IConfiguration config)
    {
        services.ConfigureServices();
        services.ConfigureProviders();
        services.ConfigureHangfire(config);
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

    private static void ConfigureHangfire(this IServiceCollection services, IConfiguration config)
    {
        var connectionString = config.GetConnectionString("DefaultConnection");
        services.AddHangfire(configuration => 
            configuration.UsePostgreSqlStorage(options => options.UseNpgsqlConnection(connectionString)));
        services.AddHangfireServer();
        services.AddScoped<DatabaseCleanupService>();
        services.AddSingleton<HangfireService>();
    }
    
    public static void ConfigureAndScheduleHangfireJobs(this IApplicationBuilder app)
    {
        using (var scope = app.ApplicationServices.CreateScope())
        {
            var hangfireService = scope.ServiceProvider.GetRequiredService<HangfireService>();
            hangfireService.ConfigureHangfireJobs();
        }
    }
}