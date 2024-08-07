using Microsoft.Extensions.DependencyInjection;
using Profile.Infrastructure.Contexts;
using Profile.Domain.Interfaces;

namespace Profile.Infrastructure.Implementations
{
    public class DbCleanUpService : IDbCleanupService
    {
        private readonly IServiceProvider _serviceProvider;

        public DbCleanUpService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async void DeleteOldRecords(List<string> ids)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ProfileDbContext>();

                using (var transaction = await dbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var usersToDelete = dbContext.Users.Where(u => ids.Contains(u.Id)).ToList();
                        dbContext.Users.RemoveRange(usersToDelete);

                        var profilesToDelete = dbContext.Profiles.Where(p => ids.Contains(p.UserId)).ToList();
                        dbContext.Profiles.RemoveRange(profilesToDelete);

                        await dbContext.SaveChangesAsync();
                        await transaction.CommitAsync();
                    }
                    catch (Exception)
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            }
        }
    }
}