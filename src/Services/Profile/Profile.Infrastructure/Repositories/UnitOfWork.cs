using Profile.Domain.Repositories;
using Profile.Infrastructure.Contexts;

namespace Profile.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ProfileDbContext _dbContext;
    private IUserProfileRepository _userProfileRepository;
    private ILanguageRepository _languageRepository;
    private IGoalRepository _goalRepository;
    private IPreferenceRepository _preferenceRepository;
    private ICountryRepository _countryRepository;
    private ICityRepository _cityRepository;
    private IInterestRepository _interestRepository;
    private IEducationRepository _educationRepository;
    
    public UnitOfWork(ProfileDbContext context)
    {
        _dbContext = context;
    }

    public IUserProfileRepository ProfileRepository => _userProfileRepository ??= new UserProfileRepository(_dbContext);
    public ILanguageRepository LanguageRepository => _languageRepository ??= new LanguageRepository(_dbContext);
    public IGoalRepository GoalRepository => _goalRepository ??= new GoalRepository(_dbContext);
    public IPreferenceRepository PreferenceRepository => _preferenceRepository ??= new PreferenceRepository(_dbContext);
    public ICountryRepository CountryRepository => _countryRepository ??= new CountryRepository(_dbContext);
    public ICityRepository CityRepository => _cityRepository ??= new CityRepository(_dbContext);
    public IInterestRepository InterestRepository => _interestRepository ??= new InterestRepository(_dbContext);
    public IEducationRepository EducationRepository => _educationRepository ??= new EducationRepository(_dbContext);
    
    private bool _disposed = false;
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
        }

        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    
    public Task SaveAsync(CancellationToken cancellationToken)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}