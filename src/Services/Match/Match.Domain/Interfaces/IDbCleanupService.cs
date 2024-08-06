namespace Match.Domain.Interfaces;

public interface IDbCleanupService
{
    Task DeleteOldRecordsAsync(IEnumerable<string> profileIds, CancellationToken cancellationToken);
}