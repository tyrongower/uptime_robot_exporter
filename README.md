# Uptime Robot Prometheus Exporter

This is a Prometheus exporter for monitoring Uptime Robot data. It collects metrics from Uptime Robot and makes them
available for Prometheus to scrape.

## Features

- Exports Uptime Robot monitor statuses.
- Exports response times.
- Supports multiple monitors.
- Easy configuration.

## Requirements

- .NET 8.0
- ASP.NET Core

## Docker

You can run the Uptime Robot Prometheus Exporter using Docker with the following command:

```sh
docker run -d -p 5000:5000 --name uptime_robot_exporter -e UPTIME_ROBOT_API_KEY=your-api-key tyrongower/uptime_robot_exporter
```

## Docker Compose

You can also run the Uptime Robot Prometheus Exporter using Docker Compose. Below is an example `docker-compose.yml`
file:

```yaml
version: '3.7'

services:
  uptime_robot_exporter:
    image: tyrongower/uptime_robot_exporter
    container_name: uptime_robot_exporter
    environment:
      - UPTIME_ROBOT_API_KEY=your-api-key
    ports:
      - "8080:8080"
```

To start the service, navigate to the directory containing the `docker-compose.yml` file and run:

```sh
docker-compose up -d
```

## Usage

1. After running the exporter, it will be available at `http://localhost:5000/metrics`.

2. Add the exporter as a target in your Prometheus configuration:

    ```yaml
    scrape_configs:
      - job_name: 'uptime_robot'
        static_configs:
          - targets: ['localhost:5000']
    ```

3. Reload the Prometheus configuration to start scraping metrics from the exporter.

## Metrics

The exporter provides the following metrics:

- `uptime_robot_monitor_status`: Status of the Uptime Robot monitors (0 = paused, 1 = not checked yet, 2 = up, 9 = seems
  down, 8 = down).
- `uptime_robot_monitor_response_time`: Response time of the Uptime Robot monitors.

## Contributing

Contributions are welcome! Please fork the repository and submit a pull request.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.