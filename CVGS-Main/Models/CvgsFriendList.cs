using System.ComponentModel.DataAnnotations;

namespace CVGS_Main.Models
{
    public class CvgsFriendList
    {
        [Key]
        public int FriendListId { get; set; }
        public string UserId { get; set; }
    }
}
