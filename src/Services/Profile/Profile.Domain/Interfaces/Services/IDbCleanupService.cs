namespace Profile.Domain.Interfaces.Services;

public interface IDbCleanupService
{
    void DeleteOldRecords(List<string> ids);
}