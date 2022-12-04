using CVGS_Main.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;

namespace CVGS_Main.Models
{
    public class CvgsPaymentMethod
    {

        [Key]
        public int PaymentMethodId { get; set; }

        public string NameOnCard { get; set; }

        public string CardNumber { get; set; }

        public string SecurityCode { get; set; }

        public DateTime ExpiryDate { get; set; }

        public string UserId { get; set; }
    }
}
