using System.Text.Json.Serialization;

namespace uptime_robot_exporter.Api;

public class Pagination
{
    [JsonPropertyName("offset")]
    public int Offset { get; set; }

    [JsonPropertyName("limit")]
    public int Limit { get; set; }

    [JsonPropertyName("total")]
    public int Total { get; set; }
}