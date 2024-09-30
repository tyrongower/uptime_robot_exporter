using System.Text.Json.Serialization;

namespace uptime_robot_exporter.Api;

public class Monitor
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("friendly_name")]
    public string FriendlyName { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }

    [JsonPropertyName("type")]
    public int Type { get; set; }

    [JsonPropertyName("sub_type")]
    public object SubType { get; set; }

    [JsonPropertyName("keyword_type")]
    public int? KeywordType { get; set; }

    [JsonPropertyName("keyword_case_type")]
    public int? KeywordCaseType { get; set; }

    [JsonPropertyName("keyword_value")]
    public string KeywordValue { get; set; }

    [JsonPropertyName("http_username")]
    public string HttpUsername { get; set; }

    [JsonPropertyName("http_password")]
    public string HttpPassword { get; set; }

    [JsonPropertyName("port")]
    public object Port { get; set; }

    [JsonPropertyName("interval")]
    public int Interval { get; set; }

    [JsonPropertyName("timeout")]
    public int Timeout { get; set; }

    [JsonPropertyName("status")]
    public int Status { get; set; }

    [JsonPropertyName("create_datetime")]
    public int CreateDatetime { get; set; }

    [JsonPropertyName("response_times")]
    public List<ResponseTime> ResponseTimes { get; set; }

    [JsonPropertyName("average_response_time")]
    public string AverageResponseTime { get; set; }
}