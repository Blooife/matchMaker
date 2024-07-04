namespace Shared.Interfaces;

public interface ISoftDeletable
{
    DateTime? DeletedAt { get; set; }
}
