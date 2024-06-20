using Profile.Domain.Models;

namespace Profile.Domain.Repositories;

public interface IPreferenceRepository
{
    Task<Preference> UpdatePreferenceAsync(Preference preference, CancellationToken cancellationToken);
    Task<Preference?> GetPreferenceByIdAsync(string id, CancellationToken cancellationToken);
    Task ChangeIsActive(Preference preference, CancellationToken cancellationToken);
}