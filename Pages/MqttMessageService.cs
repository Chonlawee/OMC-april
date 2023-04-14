using Microsoft.AspNetCore.SignalR;
using MQTTnet.Client;
using MQTTnet;
using OMC.Data;
using OMC.Hubs;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using OMC.Models;
using Microsoft.EntityFrameworkCore;

namespace OMC.Pages
{
    public class MqttMessageService
    {
        private readonly ILogger<MqttMessageService> _logger;
        private readonly OMCContext _context;
        public MqttMessageService(ILogger<MqttMessageService> logger, OMCContext context)
        {
            _logger = logger;
            _context = context;
        }

        [BindProperty]
        public Order Order { get; set; }
      
        public async Task ProcessOrderStatusMessageAsync(int currentOrderId, string currentStatus)
        {
            try
            {
                _logger.LogInformation("Starting to process order {OrderId}", currentOrderId);

                //Find the current order
                var currentOrder = await _context.Order.FindAsync(currentOrderId);

                if (currentOrder == null)
                {
                    _logger.LogWarning($"Order not found: {currentOrderId}");
                    return;
                }

                // Check if the status has changed
                if (currentOrder.Status == currentStatus)
                {
                    _logger.LogWarning($"Status already set to {currentStatus}: {currentOrderId}");
                    return;
                }

                // Update the order status
                currentOrder.Status = currentStatus;
                _context.Order.Update(currentOrder);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Order {OrderId} processed successfully", currentOrderId);
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error processing order {currentOrderId}");
            }
        }
    }
}
