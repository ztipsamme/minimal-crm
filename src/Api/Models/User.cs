using System;
using Newtonsoft.Json;

namespace api.Models;

public abstract class User : BaseModel
{
    [JsonProperty("name")]
    public string Name { get; set; } = null!;

    [JsonProperty("phoneNumber")]
    public string PhoneNumber { get; set; } = null!;

    [JsonProperty("email")]
    public string Email { get; set; } = null!;
}