using System.ComponentModel.DataAnnotations;

namespace CVGS_Main.Models
{
    public class CvgsWishlistItems
    {
        [Key]
        public int WishlistItemId { get; set; }

        public int GameId { get; set; }

        public string UserId { get; set; }
    }
}