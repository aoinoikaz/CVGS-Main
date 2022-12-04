using CVGS_Main.Models;
using Microsoft.AspNetCore.Identity;

namespace CVGS_Main.Areas.Identity.Data;

// Add profile data for application users by adding properties to the CvgsUser class
public class CvgsUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Gender { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string MailingAddress { get; set; }
    public string ShippingAddress { get; set; }
    public bool ReceivePromotion { get; set; }
    public int FavouriteGenreId { get; set; }
    public int FavouritePlatformId { get; set; }
}

