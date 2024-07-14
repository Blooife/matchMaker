using Microsoft.Extensions.DependencyInjection;
using Profile.Infrastructure.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using Profile.Application.Kafka.Producers;
using Profile.Domain.Interfaces;
using Shared.Messages.Profile;

namespace Profile.Infrastructure.Implementations
{
    public class DbCleanUpService : IDbCleanupService
    {
        private readonly IServiceProvider _serviceProvider;

        public DbCleanUpService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async void DeleteOldRecords(IEnumerable<string> ids)
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