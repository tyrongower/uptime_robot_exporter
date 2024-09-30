using System.Text.Json.Serialization;

namespace uptime_robot_exporter.Api;

public class Error
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("parameter_name")]
    public string ParameterName { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; }
}

public class ErrorResponse
{
    [JsonPropertyName("stat")]
    public string Stat { get; set; }

    [JsonPropertyName("error")]
    public Error Error { get; set; }
}