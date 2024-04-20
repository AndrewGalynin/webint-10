
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHealthChecksUI(setup =>
{
    setup.AddHealthCheckEndpoint("health1", "/health1");
    setup.AddHealthCheckEndpoint("health2", "/health2");
    setup.AddHealthCheckEndpoint("database", "/database");
})
    .AddInMemoryStorage();

builder.Services.AddHealthChecks()
    .AddCheck("database", new DatabaseHealthCheck("Server=LAPTOP-H7RBCDUO\\SQLEXPRESS; Database=Horshevska309; Trusted_Connection=True; Encrypt=False;"))
    .AddCheck<HealthCheckService1>("health1")
    .AddCheck<HealthCheckService2>("health2");

var app = builder.Build();

app.UseRouting();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapHealthChecks("/health1", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("health1"),
    ResultStatusCodes =
    {
        [HealthStatus.Healthy] = StatusCodes.Status200OK,
        [HealthStatus.Degraded] = StatusCodes.Status200OK,
        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
    },
    ResponseWriter = WriteResponse
});

app.MapHealthChecks("/health2", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("health2"),
    ResponseWriter = WriteResponse
});

app.MapHealthChecks("/database", new HealthCheckOptions
{
    Predicate = check => !check.Tags.Contains("database"),
    ResponseWriter = WriteResponse
});

app.MapHealthChecksUI();

app.MapControllers();

app.Run();

static async Task WriteResponse(HttpContext httpContext, HealthReport result)
{
    httpContext.Response.ContentType = "application/json";

    var response = new
    {
        status = result.Status.ToString(),
        totalChecks = result.Entries.Count,
        entries = result.Entries
    };

    await httpContext.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(response));
}

