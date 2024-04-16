using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Planning_Service.Models;

public class Delivery
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public Consumer Contact { get; set; }
    public string? Address1 { get; set; }
    public string? Address2 { get; set; }
    public int PostalCode { get; set; }
    public string? City { get; set; }
    public DateTime DeliveryDate { get; set; }
}