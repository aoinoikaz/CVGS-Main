using System.ComponentModel.DataAnnotations;
using System.Security.Permissions;

namespace CVGS_Main.Models
{
    public class CvgsPublisher
    {
        [Key]
        public int PublisherId { get; set; }
        public string Name { get; set; }
    }
}
