using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using Profile.Domain.Models;
using Profile.Infrastructure.Contexts;
using Profile.Tests.Fakers;

namespace Profile.Tests.IntegrationTests.Fixtures
{
    public abstract class ClassFixture
        : IClassFixture<CustomWebApplicationFactory>, IAsyncLifetime
    {
        private readonly ProfileDbContext _dbContext;
        protected readonly HttpClient Client;
        private readonly CustomWebApplicationFactory _factory;

        protected ClassFixture(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _dbContext = factory.Services.CreateScope().ServiceProvider.GetRequiredService<ProfileDbContext>();
            Client = factory.CreateClient();
        }
        
        protected void AddJwtTokenToHeader(string jwtToken)
        {
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
        }
        
        protected async Task AddEntitiesToDbAsync<T>(List<T> entities) where T: class
        {
            _dbContext.AddRange(entities);
            await _dbContext.SaveChangesAsync();
        }
        
        protected async Task AddEntitiesToDbAsync<T>(T entity) where T : class
        {
            _dbContext.Add(entity);
            await _dbContext.SaveChangesAsync();
        }

        protected async Task UpdateDbEntityAsync<T>(T entity) where T: class
        {
            _dbContext.Update(entity);
            await _dbContext.SaveChangesAsync();
        }
        
        protected async Task<UserProfile> AddValidProfileToDbAsync()
        {
            var user = UserFakers.CreateUser().Generate();
            var country = CountryFakers.CreateCountry().Generate();
            var city = CityFakers.CreateCity().Clone().RuleFor(c => c.CountryId, country.Id).Generate();
            var profile = ProfileFakers.CreateEmptyUserProfile()
                .RuleFor(p=>p.CityId, city.Id)
                .RuleFor(p=>p.UserId, user.Id)
                .Generate();

            await AddEntitiesToDbAsync(user);
            await AddEntitiesToDbAsync(country);
            await AddEntitiesToDbAsync(city);
            await AddEntitiesToDbAsync(profile);

            return profile;
        }
        
        public async Task InitializeAsync()
        {
            await _factory.ResetDatabase();
            await _factory.ResetRedisAsync();
            AddJwtTokenToHeader(_factory.JwtToken);
        }
        
        public async Task DisposeAsync()
        {
            Client.Dispose();
            await _dbContext.DisposeAsync();
        }
    }
}