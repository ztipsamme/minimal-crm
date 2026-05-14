using System;
using Newtonsoft.Json;

namespace api.Models;

public abstract class BaseModel
{
    [JsonProperty("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [JsonProperty("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [JsonProperty("updatedAt")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}
