using Microsoft.EntityFrameworkCore;
using Profile.Domain.Models;
using Profile.Domain.Repositories;
using Profile.Infrastructure.Contexts;

namespace Profile.Infrastructure.Repositories;

public class PreferenceRepository(ProfileDbContext _dbContext) : IPreferenceRepository
{
    public async Task<Preference> UpdatePreferenceAsync(Preference preference, CancellationToken cancellationToken)
    {
        _dbContext.Preferences.Update(preference);
        
        return preference;
    }
    
    public async Task<Preference?> GetPreferenceByIdAsync(string id, CancellationToken cancellationToken)
    {
        return await _dbContext.Preferences.AsNoTracking().FirstOrDefaultAsync(p => p.ProfileId == id, cancellationToken);
    }
    
    public async Task ChangeIsActive(Preference preference, CancellationToken cancellationToken)
    {
        preference.IsActive = !preference.IsActive;
        _dbContext.Preferences.Update(preference);
    }
}