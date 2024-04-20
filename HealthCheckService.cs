using Microsoft.Extensions.Diagnostics.HealthChecks;

public class HealthCheckService1 : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            return Task.FromResult(
                HealthCheckResult.Healthy("The service is up and running 1."));
        }
        catch (Exception)
        {
            return Task.FromResult(
                new HealthCheckResult(
                    context.Registration.FailureStatus, "The service is down 1."));
        }
    }
}
public class HealthCheckService2 : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            return Task.FromResult(
                HealthCheckResult.Healthy("The service is up and running 2."));
        }
        catch (Exception)
        {
            return Task.FromResult(
                new HealthCheckResult(
                    context.Registration.FailureStatus, "The service is down 2."));
        }
    }
}
