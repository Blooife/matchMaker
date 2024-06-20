namespace Profile.Domain.Repositories;

public interface IUnitOfWork : IDisposable
{
    IUserProfileRepository ProfileRepository { get; }
    ILanguageRepository LanguageRepository { get; }
    IGoalRepository GoalRepository { get; }
    IPreferenceRepository PreferenceRepository { get; }
    ICountryRepository CountryRepository { get; }
    ICityRepository CityRepository { get; }
    IInterestRepository InterestRepository { get; }
    IEducationRepository EducationRepository { get; }
    IImageRepository ImageRepository { get; }
    Task SaveAsync(CancellationToken cancellationToken);
}