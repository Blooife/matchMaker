namespace Profile.Domain.Interfaces;

public interface IDbCleanupService
{
    void DeleteOldRecords(IEnumerable<string> ids);
}