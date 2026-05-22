using System;
using Newtonsoft.Json;

namespace Domain.Models;

public class User
{
    [JsonProperty("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [JsonProperty("role")]
    public string Role { get; set; } = null!;


    [JsonProperty("name")]
    public string Name { get; set; } = null!;

    [JsonProperty("phoneNumber")]
    public string PhoneNumber { get; set; } = null!;

    [JsonProperty("email")]
    public string Email { get; set; } = null!;


    [JsonProperty("title")]
    public string Title { get; set; } = null!;

    [JsonProperty("address")]
    public Address Address { get; set; } = null!;


    [JsonProperty("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [JsonProperty("updatedAt")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    // Partition key
    [JsonProperty("vendorId")]
    public string VendorId { get; set; } = null!;
}