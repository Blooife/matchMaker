using Match.Domain.Models;
using Match.Domain.Interfaces;
using Match.Infrastructure.Context;
using Microsoft.Extensions.Options;

namespace Match.Infrastructure.Implementations;

public class UnitOfWork : IUnitOfWork
{
    private readonly IMongoDbContext _database;

    public UnitOfWork(IMongoDbContext database, IOptions<MatchDbSettings> options)
    {
        _database = database;
        Matches = new MatchRepository(_database.GetCollection<MatchEntity>(options.Value.MatchesCollectionName));
        Profiles = new ProfileRepository(_database.GetCollection<Profile>(options.Value.ProfilesCollectionName));
        Likes = new LikeRepository(_database.GetCollection<Like>(options.Value.LikesCollectionName));
        Chats = new ChatRepository(_database.GetCollection<Chat>(options.Value.ChatsCollectionName));
        Messages = new MessageRepository(_database.GetCollection<Message>(options.Value.MessagesCollectionName));
    }

    public IMatchRepository Matches { get; }
    public IProfileRepository Profiles { get; }
    public ILikeRepository Likes { get; }
    public IChatRepository Chats { get; }
    public IMessageRepository Messages { get; }

    public void Dispose()
    {
    }
}