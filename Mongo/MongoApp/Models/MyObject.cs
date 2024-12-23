using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoApp.Models;

public class MyObject
{
    [BsonElement("_id-text"), BsonRepresentation(BsonType.String)]
    public string String { get; set; }

    [BsonElement("id"), BsonRepresentation(BsonType.String)]
    public string? Id { get; set; }
}