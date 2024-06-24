using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Match.Domain.Models;

public class Like
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string ProfileId { get; set; }
    public string TargetProfileId { get; set; }
    public bool IsLike { get; set; }
}