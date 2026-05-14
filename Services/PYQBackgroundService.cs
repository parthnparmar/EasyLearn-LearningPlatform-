using EasyLearn.Services;

namespace EasyLearn.Services;

public class PYQBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<PYQBackgroundService> _logger;

    public PYQBackgroundService(IServiceProvider serviceProvider, ILogger<PYQBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var pyqService = scope.ServiceProvider.GetRequiredService<IPYQService>();
                    await pyqService.ConvertExpiredExamsToPYQAsync();
                    _logger.LogInformation("PYQ conversion completed at {time}", DateTimeOffset.Now);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error converting exams to PYQ");
            }

            await Task.Delay(TimeSpan.FromMinutes(15), stoppingToken);
        }
    }
}
