using Authentication.BusinessLogic.Producers;
using Authentication.DataLayer.Contexts;
using Microsoft.Extensions.DependencyInjection;
using Shared.Messages.Authentication;

namespace Authentication.BusinessLogic.Hangfire;

public class DatabaseCleanupService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ProducerService _producerService;

    public DatabaseCleanupService(IServiceProvider serviceProvider, ProducerService producerService)
    {
        _serviceProvider = serviceProvider;
        _producerService = producerService;
    }

    public async Task DeleteOldRecords()
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AuthContext>();
            var twoMonthAgo = DateTime.UtcNow.AddMonths(-2);
            var oldRecords = dbContext.Users
                .Where(e => e.DeletedAt != null && e.DeletedAt <= twoMonthAgo)
                .ToList();
            var ids = oldRecords.Select(u => u.Id).ToList();
            foreach (var id in ids)
            {
                Console.WriteLine(id);
            }
            if (oldRecords.Any())
            {
                dbContext.Users.RemoveRange(oldRecords);
                await dbContext.SaveChangesAsync();
            }

            
            foreach (var id in ids)
            {
                Console.WriteLine(id);
            }

            var mes = new ManyUsersDeletedMessage(ids);
            Console.WriteLine(mes.UsersIds);
            await _producerService.ProduceAsync(mes);
            Console.WriteLine(new ManyUsersDeletedMessage(ids).ToString());
        }
    }
}