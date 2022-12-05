using System.ComponentModel.DataAnnotations;

namespace CVGS_Main.Models
{
    public class CvgsCheckout
    {
        [Key]
        public int CheckoutId { get; set; }

        public int CartId { get; set; }

        public int UserId { get; set; }

        public bool IsDigital { get; set; }
    }
}
