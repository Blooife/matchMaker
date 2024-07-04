using Authentication.DataLayer.Contexts;
using Authentication.DataLayer.Models;
using Authentication.DataLayer.Repositories.Implementations;
using Authentication.DataLayer.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Authentication.DataLayer.Extensions;

public static class ServicesExtension
{
    public static void ConfigureDataLayer(this IServiceCollection services, IConfiguration config)
    {
        services.ConfigureDbContext(config);
        services.ConfigureIdentity();
        services.ConfigureRepositories();
    }
    
    private static void ConfigureDbContext(this IServiceCollection services, IConfiguration config)
    {
        var connectionString = config.GetConnectionString("DefaultConnection");
        services.AddDbContext<AuthContext>(options => options.UseNpgsql(connectionString));
    }
    
    private static void ConfigureIdentity(this IServiceCollection services)
    {
        services.AddIdentity<User, Role>(o =>
            {
                o.Password.RequireDigit = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
            })
            .AddEntityFrameworkStores<AuthContext>()
            .AddDefaultTokenProviders();
    }
    
    private static void ConfigureRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
    }
}