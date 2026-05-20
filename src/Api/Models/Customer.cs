using System;
using Newtonsoft.Json;

namespace api.Models;

public class Customer : User
{
    [JsonProperty("title")]
    public string Title { get; set; } = null!;

    [JsonProperty("address")]
    public Address Address { get; set; } = null!;

    [JsonProperty("vendorId")]
    public string VendorId { get; set; } = null!;
}