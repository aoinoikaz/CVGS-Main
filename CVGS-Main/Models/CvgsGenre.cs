using System.ComponentModel.DataAnnotations;

namespace CVGS_Main.Models
{
    public class CvgsGenre
    {
        [Key]
        public int GenreId { get; set; }

        public string Type { get; set; }
    }
}
