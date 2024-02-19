using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class Worker : BackgroundService
    {

        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            this._logger = logger;
        }



        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var server = new TcpServer();
            while (!stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine("klient został połączony");
                _logger.LogInformation($"Worker running at {DateTimeOffset.Now}");
                await Task.Delay(1000,stoppingToken);
            }
            throw new NotImplementedException();
        }
    }
}
