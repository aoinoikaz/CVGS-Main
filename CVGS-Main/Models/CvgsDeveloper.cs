using System.ComponentModel.DataAnnotations;

namespace CVGS_Main.Models
{
    public class CvgsDeveloper
    {
        [Key]
        public int DeveloperId { get; set; }

        public string Name { get; set; }

    }
}
