using System.Text.Json;

namespace uptime_robot_exporter.Api;

public class UptimeRobotApiClient(
    ILogger<UptimeRobotApiClient> logger,
    HttpClient httpClient,
    MetricsService metricsService)
{

    public async Task<List<Monitor>> GetAllMonitors()
    {

        var apiKey = Environment.GetEnvironmentVariable("UPTIME_ROBOT_API_KEY");

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            logger.LogCritical("No API key provided");
            metricsService.LogErrorMetrics("No API key provided");

            return [];
        }


        var monitors = new List<Monitor>();
        try
        {
            logger.LogInformation("Calling API: GetAllMonitors");
            int remaining;
            do
            {
                var formData = new List<KeyValuePair<string, string>>
                {
                    new("format", "json"),
                    new("response_times", "1"),
                    new("response_times_limit", "1"),
                    new("offset", monitors.Count.ToString()),
                };

                HttpContent content = new FormUrlEncodedContent(formData);
                var response = await httpClient.PostAsync($"getMonitors?api_key={apiKey}", content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();

                    logger.LogError("API call failed with status code {ResponseStatusCode}: {ResponseContent}",
                        response.StatusCode, errorContent);

                    metricsService.LogErrorMetrics(response?.ReasonPhrase ??
                                                   $"Unknown Http Error {response?.StatusCode}");

                    return monitors;
                }

                var json = await response.Content.ReadAsStringAsync();
                logger.LogDebug("Content: {Json}", json);

                var monitorResult = JsonSerializer.Deserialize<GetMonitorsResult>(json);

                if (monitorResult?.Monitors == null || monitorResult.Stat == "fail")
                {
                    var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(json);
                    logger.LogError("API call failed {Error}", errorResponse?.Error?.Message);

                    metricsService.LogErrorMetrics(errorResponse?.Error?.Message ?? "");

                    return monitors;
                }

                logger.LogInformation("{MonitorsCount} monitors returned from api call", monitorResult.Monitors.Count);

                monitors.AddRange(monitorResult.Monitors);
                remaining = monitorResult.Pagination.Total - monitors.Count;

                logger.LogInformation("GetAllMonitors completed successfully, {Remaining} remaining",remaining);

            } while (remaining != 0);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error in api client fetching uptime from api {Message}", e.Message);
            metricsService.LogErrorMetrics(e.Message);
        }

        return monitors;
    }
}