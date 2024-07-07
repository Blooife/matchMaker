using Profile.Domain.Repositories;
using Profile.Infrastructure.Contexts;
using Profile.Infrastructure.Redis.Interfaces;

namespace Profile.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ProfileDbContext _dbContext;
    private readonly ICacheService _cacheService;
    private IUserProfileRepository _profileRepository;
    private ILanguageRepository _languageRepository;
    private IGoalRepository _goalRepository;
    private ICountryRepository _countryRepository;
    private ICityRepository _cityRepository;
    private IInterestRepository _interestRepository;
    private IEducationRepository _educationRepository;
    private IImageRepository _imageRepository;
    private IUserRepository _userRepository;
    
    
    public UnitOfWork(ProfileDbContext context, ICacheService cacheService)
    {
        _dbContext = context;
        _cacheService = cacheService;
    }

    public IUserProfileRepository ProfileRepository => _profileRepository ??= new UserProfileRepository(_dbContext, _cacheService);
    public ILanguageRepository LanguageRepository => _languageRepository ??= new LanguageRepository(_dbContext, _cacheService);
    public IGoalRepository GoalRepository => _goalRepository ??= new GoalRepository(_dbContext, _cacheService);
    public ICountryRepository CountryRepository => _countryRepository ??= new CountryRepository(_dbContext, _cacheService);
    public ICityRepository CityRepository => _cityRepository ??= new CityRepository(_dbContext, _cacheService);
    public IInterestRepository InterestRepository => _interestRepository ??= new InterestRepository(_dbContext, _cacheService);
    public IEducationRepository EducationRepository => _educationRepository ??= new EducationRepository(_dbContext, _cacheService);
    public IImageRepository ImageRepository => _imageRepository ??= new ImageRepository(_dbContext, _cacheService);
    public IUserRepository UserRepository => _userRepository ??= new UserRepository(_dbContext, _cacheService);
    
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