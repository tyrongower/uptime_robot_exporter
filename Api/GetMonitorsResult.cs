using System.Text.Json.Serialization;

namespace uptime_robot_exporter.Api;

public class GetMonitorsResult
{
    [JsonPropertyName("stat")]
    public string Stat { get; set; }

    [JsonPropertyName("pagination")]
    public Pagination Pagination { get; set; }

    [JsonPropertyName("monitors")]
    public List<Monitor> Monitors { get; set; }
}