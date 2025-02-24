namespace InnoSpend.Services
{
    public class CustomerDataRetentionService : IHostedService, IDisposable
    {
        private Timer? _timer;
        private readonly IServiceProvider _services;

        public CustomerDataRetentionService(IServiceProvider services)
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
            var customerService = scope.ServiceProvider.GetRequiredService<ICustomerService>();
            await customerService.DeleteInactiveCustomers();
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
