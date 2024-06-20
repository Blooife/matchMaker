using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Match.Domain.Models;

public class Profile
{
    [BsonId]
    //[BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string UserName { get; set; }
    public string UserLastName { get; set; }
    public DateTime BirthDate { get; set; }
    public int AgeFrom { get; set; }
    public int AgeTo { get; set; }
   // public Gender Gender { get; set; }
   // public Gender PreferredGender { get; set; }
   public int MaxDistance { get; set; }
}