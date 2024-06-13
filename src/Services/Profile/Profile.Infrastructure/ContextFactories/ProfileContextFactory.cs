using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Profile.Infrastructure.Contexts;

namespace Profile.Infrastructure.ContextFactories;

internal class ProfileContextFactory : IDesignTimeDbContextFactory<ProfileDbContext>
{
    public ProfileDbContext CreateDbContext(string[] args)
    {
        var basePath = Directory.GetCurrentDirectory();
        
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(basePath, "../Profile.Presentation"))
            .AddJsonFile("appsettings.json")
            .Build();

        var builder = new DbContextOptionsBuilder<ProfileDbContext>()
            .UseNpgsql(configuration.GetConnectionString("DefaultConnection"));

        return new ProfileDbContext(builder.Options);
    }
}