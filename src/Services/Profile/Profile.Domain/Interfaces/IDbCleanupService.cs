namespace Profile.Domain.Interfaces;

public interface IDbCleanupService
{
    void DeleteOldRecords(List<string> ids);
}