using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CVGS_Main.Models.View_Models
{
    public class CvgsAdminViewModel
    {

        public int GameId { get; set; }

        [Required]
        [DisplayName("Game Title")]
        public string Name { get; set; }

        [Required]
        [DisplayName("Release Date")]
        public DateTime ReleaseDate { get; set; }

        [Required]
        [DisplayName("Publisher")]
        public int PublisherId { get; set; }

        [Required]
        [DisplayName("Developer")]
        public int DeveloperId { get; set; }

        [Required]
        [DisplayName("Genre")]
        public int GenreId { get; set; }

        [Required]
        [DisplayName("Overall Score")]
        public float OverallScore { get; set; }
    }
}
