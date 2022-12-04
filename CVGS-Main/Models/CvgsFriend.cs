using System.ComponentModel.DataAnnotations;

namespace CVGS_Main.Models
{
    public class CvgsFriend
    {
        [Key]
        public int FriendId { get; set; }
        public int FriendListId { get; set; }
        public string FriendToAdd { get; set; }
    }
}
