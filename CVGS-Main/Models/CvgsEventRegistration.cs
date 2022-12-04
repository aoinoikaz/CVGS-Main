using CVGS_Main.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;

namespace CVGS_Main.Models
{
    public class CvgsEventRegistration
    {
        [Key]
        public int RegistrationId { get; set; }

        public virtual CvgsEvent CvgsEvent { get; set; }
        public int EventId { get; set; }

        public virtual CvgsUser CvgsUser {get; set;}
        public int UserId { get; set; }
    }
}
