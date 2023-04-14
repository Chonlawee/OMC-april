using Microsoft.AspNetCore.SignalR;
using OMC.Pages;

namespace OMC.Hubs
{
    public class OrderHub : Hub
    {
        private readonly ILogger<OrderHub> _logger;

        public OrderHub( ILogger<OrderHub> logger)
        {
            _logger = logger;
        }
        public async Task NotifiUpdatePosition(int OrderId, int QueuePosition)
        {
            _logger.LogInformation("NotifiUpdatePosition called with OrderId={OrderId} and QueuePosition={QueuePosition}", OrderId, QueuePosition);
            Console.WriteLine("Start NotifiUpdatePosition");
            await Clients.All.SendAsync("UpdatePosition", OrderId, QueuePosition);
        }

        public async Task NotifiUpdateStatusFromMqtt(int OrderId, string Status)
        {
            await Clients.All.SendAsync("UpdateOrderStatusAsync", OrderId, Status);
        }
    }
}
