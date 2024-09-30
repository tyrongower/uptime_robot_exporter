using System.Reflection.Metadata;
using Prometheus;
using Monitor = uptime_robot_exporter.Api.Monitor;

namespace uptime_robot_exporter;

public class MetricsService
{
    private readonly Gauge _statusGauge;
    private const string  StatusMetricName = "uptime_robot_monitor_status";
    private const string  StatusMetricHelp = "Uptime Robot Monitor Status";

    private readonly Gauge _avgResponseTimeGauge;
    private const string  ResponseTimeMetricName = "uptime_robot_monitor_response_time";
    private const string  ResponseTimeMetricHelp = "Uptime Robot Monitor Response Time";

    private readonly Gauge _responseTimeGauge;
    private const string  AvgResponseTimeMetricName = "uptime_robot_monitor_response_time_avg";
    private const string  AvgResponseTimeMetricHelp = "Average Uptime Robot Monitor Response Time";

    private readonly Gauge _errorsGauge;
    private const string  ErrorsMetricName = "uptime_robot_monitor_errors";
    private const string  ErrorsMetricHelp = "Average Uptime Robot Monitor Errors";

    public MetricsService(ILogger<MetricsService> logger)
    {
        _responseTimeGauge = Metrics.CreateGauge(StatusMetricName, StatusMetricHelp, "id", "url", "name");;
        _statusGauge = Metrics.CreateGauge(ResponseTimeMetricName, ResponseTimeMetricHelp, "id", "url", "name");
        _avgResponseTimeGauge = Metrics.CreateGauge(AvgResponseTimeMetricName, AvgResponseTimeMetricHelp, "id", "url", "name");
        _errorsGauge = Metrics.CreateGauge(ErrorsMetricName, ErrorsMetricHelp, "error");
    }

    public void LogMonitorMetrics(Monitor monitor)
    {
        _statusGauge.WithLabels(monitor.Id.ToString(), monitor.Url, monitor.FriendlyName).Set(monitor.Status);

        if (double.TryParse(monitor.AverageResponseTime, out var averageResponseTime))
            _avgResponseTimeGauge.WithLabels(monitor.Id.ToString(), monitor.Url, monitor.FriendlyName)
                .Set(averageResponseTime);

        var responseTime = monitor.ResponseTimes.OrderByDescending(a => a.Datetime).FirstOrDefault();

        if (responseTime != null)
            _responseTimeGauge.WithLabels(monitor.Id.ToString(), monitor.Url, monitor.FriendlyName)
                .Set(responseTime.Value);


    }

    public void LogErrorMetrics(string error)
    {
        _errorsGauge.WithLabels(error).Inc();
    }

}