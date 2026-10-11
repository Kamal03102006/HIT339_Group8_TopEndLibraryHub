using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TopEndLibraryHub.Services
{
    public class NotificationWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly TimeProvider _timeProvider;
        private readonly ILogger<NotificationWorker> _logger;

        public NotificationWorker(
            IServiceScopeFactory scopeFactory,
            TimeProvider timeProvider,
            ILogger<NotificationWorker> logger)
        {
            _scopeFactory = scopeFactory;
            _timeProvider = timeProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "Simulated loan notification checks started with a one-minute interval.");

            try
            {
                // Catch up eligible active loans when the application starts.
                await CheckLoansAsync(stoppingToken);

                using var timer = new PeriodicTimer(
                    TimeSpan.FromMinutes(1), _timeProvider);

                // Await each check so this worker never overlaps its own scans.
                while (await timer.WaitForNextTickAsync(stoppingToken))
                {
                    await CheckLoansAsync(stoppingToken);
                }
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                // Normal application shutdown.
            }
            finally
            {
                _logger.LogInformation("Simulated loan notification checks stopped.");
            }
        }

        private async Task CheckLoansAsync(CancellationToken stoppingToken)
        {
            try
            {
                // Hosted services are singletons; each scan needs its own
                // scoped NotificationService and database context.
                await using var scope = _scopeFactory.CreateAsyncScope();
                var service = scope.ServiceProvider
                    .GetRequiredService<NotificationService>();

                var created = await service.ProcessActiveLoansAsync(stoppingToken);
                if (created > 0)
                {
                    _logger.LogInformation(
                        "Created {RecordCount} simulated notification records.", created);
                }
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception exception)
            {
                // A temporary database failure must not stop future checks.
                _logger.LogError(exception,
                    "A simulated notification check failed. The worker will retry at the next interval.");
            }
        }
    }
}
