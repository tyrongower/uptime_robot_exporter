using System.Reflection.Metadata;
using Prometheus;
using Monitor = uptime_robot_exporter.Api.Monitor;

namespace uptime_robot_exporter;

public class MetricsService(ILogger<MetricsService> logger)
{
    private readonly Gauge _statusGauge =
        Metrics.CreateGauge(ResponseTimeMetricName, ResponseTimeMetricHelp, "id", "url", "name");

    private const string StatusMetricName = "uptime_robot_monitor_status";
    private const string StatusMetricHelp = "Uptime Robot Monitor Status";

    private readonly Gauge _avgResponseTimeGauge =
        Metrics.CreateGauge(AvgResponseTimeMetricName, AvgResponseTimeMetricHelp, "id", "url", "name");

    private const string ResponseTimeMetricName = "uptime_robot_monitor_response_time";
    private const string ResponseTimeMetricHelp = "Uptime Robot Monitor Response Time";

    private readonly Gauge _responseTimeGauge =
        Metrics.CreateGauge(StatusMetricName, StatusMetricHelp, "id", "url", "name");

    private const string AvgResponseTimeMetricName = "uptime_robot_monitor_response_time_avg";
    private const string AvgResponseTimeMetricHelp = "Average Uptime Robot Monitor Response Time";

    private readonly Gauge _errorsGauge = Metrics.CreateGauge(ErrorsMetricName, ErrorsMetricHelp, "error");
    private const string ErrorsMetricName = "uptime_robot_monitor_errors";
    private const string ErrorsMetricHelp = "Average Uptime Robot Monitor Errors";

    public void LogMonitorMetrics(Monitor monitor)
    {
        var labels = new[] { monitor.Id.ToString(), monitor.Url, monitor.FriendlyName };

        // ReSharper disable once CoVariantArrayConversion
        logger.LogInformation("Logging metrics for monitor {@Labels}",labels);

        _statusGauge.WithLabels(labels).Set(monitor.Status);

        if (double.TryParse(monitor.AverageResponseTime, out var averageResponseTime))
            _avgResponseTimeGauge.WithLabels(labels)
                .Set(averageResponseTime);

        var responseTime = monitor.ResponseTimes.OrderByDescending(a => a.Datetime).FirstOrDefault();

        if (responseTime != null)
            _responseTimeGauge.WithLabels(labels).Set(responseTime.Value);

    }

    public void LogErrorMetrics(string error)
    {
        _errorsGauge.WithLabels(error).Inc();
    }

}