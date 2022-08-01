using Microsoft.Extensions.Diagnostics.HealthChecks;
using MongoDB.Driver;
using MongoDB.Driver.Core.Servers;

class MongoHealthCheck : IHealthCheck
{
    private IConfiguration _configuration;

    public MongoHealthCheck(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var connectionString = _configuration.GetSection(MongoConnectionSettings.position).Get<MongoConnectionSettings>().MONGO_URL;

            var client = new MongoClient(connectionString);
            var state = client.Cluster.Description.Servers.Single().State;
            if (state == ServerState.Connected)
            {
                return Task.FromResult(HealthCheckResult.Healthy("MongoDB is healthy"));
            }
            return Task.FromResult(HealthCheckResult.Unhealthy("MongoDB is unhealthy"));
        }
        catch (Exception ex)
        {
            return Task.FromResult(HealthCheckResult.Unhealthy("MongoDBs is unhealthy", ex));
        }
    }

}

