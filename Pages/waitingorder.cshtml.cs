using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using OMC.Data;
using OMC.Models;
using OMC.Hubs;
using MQTTnet;
using MQTTnet.Client;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;
using Omise;

namespace OMC.Pages
{
    public class waitingOrderModel : PageModel
    {


        private readonly OMCContext _context;
        private readonly IHubContext<OrderHub> _orderHub;
        private readonly ILogger<waitingOrderModel> _logger;
        private readonly IMemoryCache _cache;
        private System.Timers.Timer _timer;

        public waitingOrderModel(OMCContext context, IHubContext<OrderHub> orderHub, ILogger<waitingOrderModel> logger, IMemoryCache cache)
        {
            _context = context;
            _orderHub = orderHub;
            _logger = logger;
            _cache = cache; 
        }
        [BindProperty]
        public Order Order { get; set; }
        public int QueuePosition { get; set; }
        public int MqttOrderId { get; set; }
        public string MqttOrderStatus { get; set; }


        public async Task<IActionResult> OnGetAsync(int id)
        {


            if (id == null)
            {
                return NotFound();
            }
            var order = await _context.Order.FirstOrDefaultAsync(m => m.OrderID == id);

            if (order == null)
            {
                return NotFound();
            }

            Order = order;

            var messageOrderID = _cache.Get<int>("MqttOrderId");
            var messageOrderStatus = _cache.Get<string?>("MqttOrderStatus");
             
            await _context.SaveChangesAsync();

            await MqttMessageHanddle();

            await UpdateOrderStatusAsync(id);

            return Page();
        }

        public async Task MqttMessageHanddle( )
        {
            var factory = new MqttFactory();
            var client = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithTcpServer("broker.mqttdashboard.com")
                .Build();

            await client.ConnectAsync(options);

            var mqttSubscribeOptions = factory.CreateSubscribeOptionsBuilder()
                .WithTopicFilter(f => f.WithTopic("OMC/MSG3"))
                .Build();

            await client.SubscribeAsync(mqttSubscribeOptions);

            client.ApplicationMessageReceivedAsync += async (e) =>
            {
                var payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);

                try
                {
                    var message = JsonSerializer.Deserialize<Order>(payload);

                    if (message == null)
                    {
                        _logger.LogWarning($"Received message could not be deserialized: {payload}");
                        return;
                    }

                    else
                    {
                        Console.WriteLine($"Recive message {payload}");

                        MqttOrderId = message.OrderID;
                            
                        MqttOrderStatus = message.Status;

                       _cache.Set("MqttOrderId", MqttOrderId);

                        _cache.Set("MqttOrderStatus", MqttOrderStatus);
          
                    }
                }
                catch (JsonException ex)
                {
                    _logger.LogWarning($"Error deserializing message: {ex.Message}");
                }
            };
        }
            public async Task UpdateOrderStatusAsync(int currentOrderId)
            {
                // Find the current order
                try
                {
                    _logger.LogInformation("Starting to process order {OrderId}", currentOrderId);

                    var currentOrder = await _context.Order.FindAsync(currentOrderId);

                    if (currentOrder == null || currentOrder.Status != "Waiting")
                    {
                        await UpdateQueuePositionAsync(currentOrderId);
                        return;
                    }

                    // Find the previous order with status "done"
                    var previousOrder = await _context.Order
                        .OrderByDescending(o => o.OrderID)
                        .FirstOrDefaultAsync(o => o.OrderID < currentOrderId && o.Status == "Done");

                    if (previousOrder == null)
                    {
                        return;
                    }

                    var NextDoneOrder = await _context.Order
                       .OrderByDescending(o => o.OrderID)
                       .FirstOrDefaultAsync(o => o.OrderID == previousOrder.OrderID + 1);

                    if (NextDoneOrder == null)
                    {
                        return;
                    }

                    // Update the current order status to "OnProcess"
                    NextDoneOrder.Status = "OnProcess";
                    _context.Order.Update(NextDoneOrder);
                    await _context.SaveChangesAsync();

                    await UpdateQueuePositionAsync(currentOrderId);
                   
                    _logger.LogInformation("Order {OrderId} processed successfully", currentOrderId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error processing order {currentOrderId}");
                }
            }


        //IHUB
        public async Task UpdateQueuePositionAsync(int currentOrderId)
        {
            // Update the queue positionxx
            var previousOrder = await _context.Order
           .OrderByDescending(o => o.OrderID)
           .FirstOrDefaultAsync(o => o.OrderID < currentOrderId && o.Status != "Done" && o.Status == "OnProcess");

            var newQueuePosition = previousOrder == null ? 0 : (currentOrderId - previousOrder.OrderID);

            Order.QueuePosition = newQueuePosition;

            QueuePosition = newQueuePosition;

            await _context.SaveChangesAsync();


            await _orderHub.Clients.All.SendAsync("NotifiUpdatePosition", currentOrderId, newQueuePosition);

            Console.WriteLine($"{currentOrderId} + {newQueuePosition}");
            // Call the UpdateQueuePosition method on the hub to send the updated queue position to all connected clients
        }
        
        public async Task<IActionResult> OnPostUpdateOrderStatusAjax(int id)
        {
            await UpdateOrderStatusAsync(id);
            return new EmptyResult();
        }

        public async Task<IActionResult> OnPostUpdateOrderFromMqtt(int MQTTOrderID, string MQttOrderStatus)
        {
            try
            {
                MQTTOrderID = _cache.Get<int>("MqttOrderId");
                MQttOrderStatus = _cache.Get<string>("MqttOrderStatus");

                // Check if currentOrderId or currentStatus is null
                if (MQTTOrderID == 0 || MQttOrderStatus == null)
                {
                    _logger.LogWarning($"Null parameter received - currentOrderId: {MQTTOrderID}, currentStatus: {MQttOrderStatus}");
                    return new EmptyResult();
                }

                _logger.LogInformation("Starting to process order {OrderId}", MQTTOrderID);
                {
                    //Find the current order
                    var Editorder = await _context.Order.FindAsync(MQTTOrderID);

                    if (Editorder == null)
                    {
                        _logger.LogWarning($"Order not found: {MQTTOrderID}");
                        
                        
                    }

                    // Check if the status has changed
                    if (Editorder.Status == MQttOrderStatus)
                    {
                        _logger.LogWarning($"Status already set to {MQttOrderStatus}: {MQTTOrderID}");
                        
                    }

                    // Update the order status
                    Editorder.Status = MQttOrderStatus;
                    _context.Order.Update(Editorder);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Order {OrderId} processed successfully", MQTTOrderID);

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error processing order {MQttOrderStatus}");
            }
            return new EmptyResult();
        }
   
    }


}
