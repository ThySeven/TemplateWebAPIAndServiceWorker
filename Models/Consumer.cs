using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Planning_Service.Models;

public class Consumer
{
    public string? Name { get; set; }
    public string? Address1 { get; set; }
    public string? Address2 { get; set; }
    public int PostalCode { get; set; }
    public string? City { get; set; }
    public string? ContactName { get; set; }
    public string? TaxNumber { get; set; }
}