using System;
using Newtonsoft.Json;

namespace api.Models;

public class Address
{
    [JsonProperty("street")]
    public string Street { get; set; } = null!;

    [JsonProperty("city")]
    public string City { get; set; } = null!;

    [JsonProperty("postalCode")]
    public string PostalCode { get; set; } = null!;

    [JsonProperty("country")]
    public string Country { get; set; } = null!;

}