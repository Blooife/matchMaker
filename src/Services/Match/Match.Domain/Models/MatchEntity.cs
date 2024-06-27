using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Match.Domain.Models;

public class MatchEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string FirstProfileId { get; set; }
    public string SecondProfileId { get; set; }
    public DateTime Timestamp { get; set; }
}