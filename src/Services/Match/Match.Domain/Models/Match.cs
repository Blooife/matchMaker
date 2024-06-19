using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Match.Domain.Models;

public class Match
{
    [BsonId]
    public int Id { get; set; }
    public string ProfileId1 { get; set; }
    public string ProfileId2 { get; set; }
}