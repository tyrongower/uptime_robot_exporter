using System.Text.Json.Serialization;

namespace uptime_robot_exporter.Api;

public class ResponseTime
{
    [JsonPropertyName("datetime")]
    public int Datetime { get; set; }

    [JsonPropertyName("value")]
    public int Value { get; set; }
}