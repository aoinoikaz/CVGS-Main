using System.ComponentModel.DataAnnotations;

namespace CVGS_Main.Models
{
    public class CvgsPlatform
    {
        [Key]
        public int PlatformId { get; set; }
        public string Name { get; set; }
    }
}
