using System.ComponentModel.DataAnnotations;

namespace CVGS_Main.Models
{
    public class CvgsLineItem
    {
        [Key]
        public int LineItemId { get; set; }

        public int CartId { get; set; }

        public int GameId { get; set; }
    }
}
