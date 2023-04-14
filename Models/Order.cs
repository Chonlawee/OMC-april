using Microsoft.Build.Framework;
using OMC.Data;
namespace OMC.Models
    {
        public class Order
        {
       
            public int OrderID { get; set; }
            [Required]
            public string? UserID { get; set; }
            public int ProductID { get; set; }
            public int Cup_Amount { get; set; } 
            public int  Total { get; set; } 
            public string Status { get; set; } = "Waiting";
            public int QueuePosition { get; set; } // new property for queue positon 
            public int IsStock { get; set; } = 0;
            public Product? Product { get; set; }

            [Required]
            public ApplicationUser? User { get; set; }

            public DateTime Created { get; set; } // new property to store creation timestamp

            public DateTime Modified { get; set; }
            public DateTime? Deleted { get; set; } 
    }
    }
