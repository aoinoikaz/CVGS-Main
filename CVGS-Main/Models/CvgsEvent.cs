using System.ComponentModel.DataAnnotations;

namespace CVGS_Main.Models
{
    public class CvgsEvent
    {
        [Key]
        public int EventId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime ScheduledTime { get; set; }
    }
}
