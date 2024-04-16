using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Planning_Service.Models;

public class Delivery
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public Guid Id { get; set; }
    public Consumer Contact { get; set; }
    public string? Address1 { get; set; }
    public string? Address2 { get; set; }
    public short PostalCode { get; set; }
    public string? City { get; set; }
    public DateTime DeliveryDate { get; set; }
}