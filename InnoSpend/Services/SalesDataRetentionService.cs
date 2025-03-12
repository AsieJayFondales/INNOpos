// Services/SalesDataRetentionService.cs
//v56/v3.0.0

using InnoSpend.Services;
using Microsoft.EntityFrameworkCore;

namespace InnoSpend.Services
{
    public class SalesDataRetentionService : IHostedService, IDisposable
    {
        private Timer? _timer;
        private readonly IServiceProvider _services;

        public SalesDataRetentionService(IServiceProvider services)
        {
            _services = services;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromDays(1));
            return Task.CompletedTask;
        }

        private async void DoWork(object? state)
        {
            using var scope = _services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var sixMonthsAgo = DateTime.Now.AddMonths(-6);
            var oldReports = await context.Sales
                .Where(s => s.SaleDate < sixMonthsAgo)
                .ToListAsync();

            context.Sales.RemoveRange(oldReports);
            await context.SaveChangesAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}