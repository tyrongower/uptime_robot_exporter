using uptime_robot_exporter.Api;

namespace uptime_robot_exporter;

public class MetricsApiMiddleware(RequestDelegate next, ILogger<MetricsApiMiddleware> logger,UptimeRobotApiClient client, MetricsService metricsService)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            var results = await client.GetAllMonitors();
            foreach (var monitor in results) metricsService.LogMonitorMetrics(monitor);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error in MetricsApiMiddleware fetching uptime from api {Message}", e.Message);
        }

        await next(context);
    }
}