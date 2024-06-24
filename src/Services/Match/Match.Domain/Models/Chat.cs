using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Match.Domain.Models;

public class Chat
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string ProfileId1 { get; set; }
    public string ProfileId2 { get; set; }
    public DateTime LastMessageTimestamp { get; set; }
}