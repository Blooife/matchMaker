using Authentication.DataLayer.Contexts;
using Microsoft.Extensions.DependencyInjection;

namespace Authentication.BusinessLogic.Hangfire;

public class DatabaseCleanupService
{
    private readonly IServiceProvider _serviceProvider;

    public DatabaseCleanupService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void DeleteOldRecords()
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AuthContext>();
            var twoMonthAgo = DateTime.UtcNow.AddMonths(-2);
            var oldRecords = dbContext.Users
                .Where(e => e.DeletedAt != null && e.DeletedAt <= twoMonthAgo)
                .ToList();

            if (oldRecords.Any())
            {
                dbContext.Users.RemoveRange(oldRecords);
                dbContext.SaveChanges();
            }
        }
    }
}