using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Shared.Interfaces;

namespace Match.Domain.Models;

public class Chat : ISoftDeletable
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string FirstProfileId { get; set; }
    public string SecondProfileId { get; set; }
    public DateTime LastMessageTimestamp { get; set; }
    public DateTime? DeletedAt { get; set; }
}