using System.ComponentModel.DataAnnotations;

namespace CVGS_Main.Models
{
    public class CvgsReview
    {
        [Key]
        public int ReviewId { get; set; }
        public int GameId { get; set; }
        public string UserId { get; set; }
        public string Description { get; set; }
        public bool IsApproved { get; set; }
    }
}
