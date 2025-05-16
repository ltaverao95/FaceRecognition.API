namespace Faces.WebMvc.Core.HealthChecks
{
    using Microsoft.Extensions.Diagnostics.HealthChecks;

    public class SiteHealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(
                HealthCheckResult.Healthy("A healthy WebApp."));
        }
    }
}