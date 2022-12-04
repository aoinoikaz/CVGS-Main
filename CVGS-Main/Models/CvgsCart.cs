using System.ComponentModel.DataAnnotations;

namespace CVGS_Main.Models
{
    public class CvgsCart
    {
        [Key]
        public int CartId { get; set; }

        public string UserId { get; set; }

        //public virtual CvgsLineItem CvgsLineItem { get; set; }
    }
}
