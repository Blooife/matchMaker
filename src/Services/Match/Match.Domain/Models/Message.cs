using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Shared.Interfaces;

namespace Match.Domain.Models;

public class Message : ISoftDeletable
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string SenderId { get; set; }
    public string Content { get; set; }
    public DateTime Timestamp { get; set; }
    
    public string ChatId { get; set; }
    public DateTime? DeletedAt { get; set; }
}