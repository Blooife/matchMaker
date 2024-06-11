using Authentication.DataLayer.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Authentication.DataLayer.ContextFactories;

internal class AuthContextFactory : IDesignTimeDbContextFactory<AuthContext>
{
    public AuthContext CreateDbContext(string[] args)
    {
        var basePath = Directory.GetCurrentDirectory();
        
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(basePath, "../Authentication.API"))
            .AddJsonFile("appsettings.json")
            .Build();

        var builder = new DbContextOptionsBuilder<AuthContext>()
            .UseNpgsql(configuration.GetConnectionString("DefaultConnection"));

        return new AuthContext(builder.Options);
    }
}