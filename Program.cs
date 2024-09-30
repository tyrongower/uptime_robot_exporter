using Prometheus;
using uptime_robot_exporter;
using uptime_robot_exporter.Api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient<UptimeRobotApiClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiUrl"]);
    client.Timeout = TimeSpan.FromSeconds(10);
});
builder.Services.UseHttpClientMetrics();
builder.Services.AddSingleton<MetricsService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthorization();
app.MapControllers();
app.MapMetrics();
app.UseMiddleware<MetricsApiMiddleware>();
app.Run();