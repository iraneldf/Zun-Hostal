namespace API.Application.Services
{
    public class WindowService : BackgroundService
    {
        public ILogger Logger { get; }

        public WindowService(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger<WindowService>();
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Logger.LogInformation("ZunApplication is starting.");
            stoppingToken.Register(() => Logger.LogInformation("ZunApplication is stopping."));
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
            Logger.LogInformation("ZunApplication has stopped.");
        }
    }

}
