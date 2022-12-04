using CVGS_Main.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;

namespace CVGS_Main.Models
{
    public class CvgsEventRegistration
    {
        [Key]
        public int RegistrationId { get; set; }

        public int EventId { get; set; }

        public string UserId { get; set; }
    }
}
