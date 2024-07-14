using Match.Domain.Interfaces;
using Match.Domain.Models;
using Match.Infrastructure.Context;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Match.Infrastructure.Implementations;

public class DbCleanupService : IDbCleanupService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IOptions<MatchDbSettings> _options;

    public DbCleanupService(IServiceProvider serviceProvider, IOptions<MatchDbSettings> options)
    {
        _serviceProvider = serviceProvider;
        _options = options;
    }

    public async Task DeleteOldRecordsAsync(IEnumerable<string> profileIds, CancellationToken cancellationToken)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var mongoContext = scope.ServiceProvider.GetRequiredService<IMongoDbContext>();
            var profilesCollection = mongoContext.GetCollection<Profile>(_options.Value.ProfilesCollectionName);
            var matchesCollection = mongoContext.GetCollection<MatchEntity>(_options.Value.MatchesCollectionName);
            var chatsCollection = mongoContext.GetCollection<Chat>(_options.Value.ChatsCollectionName);

            var profileFilter = Builders<Profile>.Filter.In(p => p.Id, profileIds);
            await profilesCollection.DeleteManyAsync(profileFilter, cancellationToken);

            var matchFilter = Builders<MatchEntity>.Filter.Or(
                Builders<MatchEntity>.Filter.In(m => m.FirstProfileId, profileIds),
                Builders<MatchEntity>.Filter.In(m => m.SecondProfileId, profileIds));
            await matchesCollection.DeleteManyAsync(matchFilter, cancellationToken);

            var chatFilter = Builders<Chat>.Filter.Or(
                Builders<Chat>.Filter.In(c => c.FirstProfileId, profileIds),
                Builders<Chat>.Filter.In(c => c.SecondProfileId, profileIds));
            await chatsCollection.DeleteManyAsync(chatFilter, cancellationToken);
                    
        }
    }
}
