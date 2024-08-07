using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.GeoJsonObjectModel;
using Shared.Constants;
using Shared.Interfaces;

namespace Match.Domain.Models;

public class Profile : ISoftDeletable
{
    [BsonId]
    public string Id { get; set; }
    public DateTime BirthDate { get; set; }
    public int AgeFrom { get; set; }
    public int AgeTo { get; set; }
    public Gender Gender { get; set; }
    public Gender PreferredGender { get; set; }
    public int MaxDistance { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public GeoJsonPoint<GeoJson2DCoordinates>? Location { get; set; }
    public DateTime? DeletedAt { get; set; }
}