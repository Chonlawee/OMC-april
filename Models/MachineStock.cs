using Microsoft.Build.Framework;

namespace OMC.Models
{
    public class MachineStock
    {
        public int MachineStockID { get; set; }

        public int MachineID { get; set; }
        public Machine Machine { get; set; } 

        public int MilkStock { get; set; }

        public int WaterStock { get; set; }

        public int SyrubStock { get; set; }

        public int CoffeeStock { get; set; }

        public DateTime Created { get; set; } = DateTime.Now;

        public DateTime Modified { get; set; } = DateTime.Now;


        public DateTime? Deleted { get; set; }

    }
}
