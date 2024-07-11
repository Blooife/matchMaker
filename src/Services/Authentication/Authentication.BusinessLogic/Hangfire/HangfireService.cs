using Hangfire;

namespace Authentication.BusinessLogic.Hangfire;

public class HangfireService : IHangfireService
{
    private readonly IRecurringJobManager _recurringJobManager;

    public HangfireService(IRecurringJobManager recurringJobManager)
    {
        _recurringJobManager = recurringJobManager;
    }

    public void ConfigureHangfireJobs()
    {
        _recurringJobManager.AddOrUpdate<DatabaseCleanupService>(
            "DeleteOldRecords",
            service => service.DeleteOldRecords(),
            Cron.Weekly);
    }
}