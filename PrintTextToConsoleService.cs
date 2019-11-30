using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace generichost
{
    internal class PrintTextToConsoleService : IHostedService, IDisposable
    {
        private readonly ILogger logger;
        private readonly IOptions<AppSettings> appSettings;
        private Timer timer;

        public PrintTextToConsoleService(ILogger<PrintTextToConsoleService> logger, IOptions<AppSettings> appSettings)
        {
            this.logger = logger;
            this.appSettings = appSettings;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Started");

            timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Stopping.");

            timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            timer?.Dispose();
        }

        private void DoWork(object state)
        {
            logger.LogInformation($"Version {appSettings.Value.Version}. Background work with text: {appSettings.Value.TextToPrint}");
        }
    }
}