using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OMC.Data;
using OMC.Models;
using Microsoft.CodeAnalysis;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using MQTTnet;
using MQTTnet.Client;
using System.Text;

namespace OMC.Pages
{
    public class OrderDetailModel : PageModel
    {
        private readonly OMCContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public OrderDetailModel(OMCContext context,UserManager<ApplicationUser> userManager)
        {

            _context = context;
            _userManager = userManager;
        }
        public Product? Product { get; set; }
        [BindProperty]
        public Order Order { get; set; }
        public Recipe Recipe { get; set; }
        [BindProperty]
        public string SelectSyrup { get; set; }
        
        [BindProperty]
        public OrderMqttModel OrderMqtt { get; set; }

        public class OrderMqttModel
        {
            public int OrderID { get; set; }
            public int ProcuctTypeNumber { get; set; }
            public int Water { get; set; }
            public int Milk { get; set; }
            public int Syrup { get; set; }
            public int CupAmount { get; set; }
            public int IsStock { get; set; }

        }   
        public async Task<IActionResult> OnGetAsync(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the user's userId

            var product = await _context.Product.FirstOrDefaultAsync(m => m.ProductID == id);

            if (id == null)
            {
                return NotFound();
            }

            
            if (product == null)
            {
                return NotFound();
            }

            else
            {
                
                Product = product;
             
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var product = await _context.Product.FirstOrDefaultAsync(p => p.ProductID == id);

            if (product == null)
            {
                return NotFound();
            }

            else

            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the user's userId
                Product = product;
                if (!ModelState.IsValid)
                {
                    
                    return Page();
                }
                /// If no recipe is found, return an error
                var recipe = await _context.Recipe
                   .Where(r => r.ProductID == id && SelectSyrup == r.RecipeName )
                   .FirstOrDefaultAsync();

                

                if (recipe == null)
               {
                   ModelState.AddModelError(string.Empty, "No recipe found for the given product and syrup values.");
                   return Page();
               }
                var syrup = recipe.Syrup;
                var warter = recipe.Water;

                // Retrieve orders that are currently in the queue
                var ordersInQueue = await _context.Order
                    .Where(o => o.Status == "Waiting" || o.Status == "OnProcess")
                    .OrderBy(o => o.QueuePosition)
                    .ToListAsync();
                // Calculate the queue position of the new order
                int maxQueuePosition = ordersInQueue.Any() ? ordersInQueue.Max(o => o.QueuePosition) : 0;
                int newQueuePosition = maxQueuePosition + 1;
                // Check if the previous queue has Status == Done
                bool isPreviousQueueDone = true;
                if (ordersInQueue.Any() && newQueuePosition > 1)
                {
                    var previousQueue = ordersInQueue.FirstOrDefault(o => o.QueuePosition == newQueuePosition - 1);
                    if (previousQueue != null && previousQueue.Status != "Done")
                    {
                        isPreviousQueueDone = false;
                    }
                }

                // Create the new order
                var neworder = new Order
                {   
                    OrderID = Order.OrderID,
                    UserID = userId,
                    ProductID = recipe.ProductID,
                    Total = product.ProductPrice*Order.Cup_Amount,
                    QueuePosition = newQueuePosition,
                    Status = isPreviousQueueDone ? "OnProcess" : "Waiting",
                    Cup_Amount = Order.Cup_Amount,
                };

               

                //Add the new order to the database
                _context.Order.Add(neworder);
                await _context.SaveChangesAsync();

                var OrderToMqtt = await (from o in _context.Order
                                         join p in _context.Product on o.ProductID equals p.ProductID
                                         join r in _context.Recipe on p.ProductID equals r.ProductID
                                         select new OrderMqttModel
                                         {
                                             OrderID = neworder.OrderID,
                                             ProcuctTypeNumber = product.ProductTypeNumber,
                                             Water = recipe.Water,
                                             Syrup = recipe.Syrup,
                                             CupAmount = neworder.Cup_Amount,
                                             IsStock = neworder.IsStock,
                                         }).FirstOrDefaultAsync();

                var mqttFactory = new MqttFactory();

                string payload = OrderToMqtt.OrderID.ToString() + " " +
                  OrderToMqtt.ProcuctTypeNumber.ToString() + " " +
                  OrderToMqtt.Water.ToString() + " " +
                  OrderToMqtt.Syrup + " " +
                  OrderToMqtt.CupAmount.ToString() + " " +
                  OrderToMqtt.IsStock.ToString();


                
                // Convert the payload to a byte array
                byte[] payloadBytes = Encoding.UTF8.GetBytes(payload);

                using (var mqttClient = mqttFactory.CreateMqttClient())
                {
                    var mqttClientOptions = new MqttClientOptionsBuilder()
                    .WithTcpServer("broker.mqttdashboard.com")
                    .Build();

                    await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);
                    var applicationMessage = new MqttApplicationMessageBuilder()
                         .WithTopic("OMC/MSG2")
                         .WithPayload(payloadBytes)  
                         .Build();

                    await mqttClient.PublishAsync(applicationMessage, CancellationToken.None);

                    await mqttClient.DisconnectAsync();

                    Console.WriteLine("MQTT application message is published");

                }
                return RedirectToPage("./waitingorder", new { id = neworder.OrderID });
            }
        }

    }
}