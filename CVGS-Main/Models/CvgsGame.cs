using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CVGS_Main.Models
{
    public class CvgsGame
    {

        [Key]
        public int GameId { get; set; }

        public string Name { get; set; }

        public DateTime ReleaseDate { get; set; }

        public int PublisherId { get; set; }
        public virtual CvgsPublisher Publisher { get; set; }

        public int DeveloperId { get; set; }
        public virtual CvgsDeveloper Developer { get; set; }

        public int GenreId { get; set; }
        public virtual CvgsGenre Genre { get; set; }

        [DisplayName("Total Likes")]
        public float OverallScore { get; set; }

        public float Price { get; set; }

    }
}
