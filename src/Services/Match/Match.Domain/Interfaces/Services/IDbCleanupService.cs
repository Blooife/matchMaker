namespace Match.Domain.Interfaces.Services;

public interface IDbCleanupService
{
    Task DeleteOldRecordsAsync(IEnumerable<string> profileIds, CancellationToken cancellationToken);
}