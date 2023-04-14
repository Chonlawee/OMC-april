using Microsoft.Build.Framework;

using OMC.Data;

namespace OMC.Models
{
    public class Machine
    {
        public int MachineID { get; set; }

        public string MachineLocation { get; set; } = string.Empty;

        public string MachineStaus { get; set; } = string.Empty;

        public DateTime Created { get; set; } = DateTime.Now;

        public DateTime Modified { get; set; } = DateTime.Now;

        public DateTime? Deleted { get; set; }
    }
}
