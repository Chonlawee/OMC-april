using OMC.Data;

namespace OMC.Models
{
    public class PaymentDetail
    {
        public int PaymentDetailID { get; set; }

        public int OrderID { get; set; }

        public int UserID { get; set; }

        public Order Order { get; set; }

        public ApplicationUser User { get; set; }

        public DateTime PaymentDate { get; set; }

        public DateTime Created { get; set; } =DateTime.Now;

        public DateTime Modifiled { get; set; } =DateTime.Now;

        public DateTime Deleted { get; set; }

    }
}
