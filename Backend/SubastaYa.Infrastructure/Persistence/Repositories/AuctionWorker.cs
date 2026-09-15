using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SubastaYa.Application.Interfaces.Service.Worker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaYa.Infrastructure.Persistence.Repositories
{
    public class AuctionWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public AuctionWorker(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested) 
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var startingService = scope.ServiceProvider.GetRequiredService<IAuctionStartingService>();
                    await startingService.HandleAsync();

                    var closingService = scope.ServiceProvider.GetRequiredService<IAuctionClosingService>();
                    await closingService.HandleAsync();
                }

                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }
        }
    }
}
