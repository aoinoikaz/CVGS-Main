// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using CVGS_Main.Areas.Identity.Data;
using CVGS_Main.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CVGS_Main.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<CvgsUser> _userManager;
        private readonly SignInManager<CvgsUser> _signInManager;
        
        private readonly CvgsDbContext _context;

        public IndexModel(
            UserManager<CvgsUser> userManager,
            SignInManager<CvgsUser> signInManager, 
            CvgsDbContext context)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public string Username { get; set; }
     
        [TempData]
        public string StatusMessage { get; set; }
 
        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name = "Mailing Address")]
            public string MailingAddress { get; set; }

            [Display(Name = "Shipping Address")]
            public string ShippingAddress { get; set; }

            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Display(Name = "Gender")]
            public string Gender { get; set; }

            [Display(Name = "Date Of Birth")]
            public DateTime DateOfBirth { get; set; }

            [Display(Name = "Receive Promotion")]
            public bool ReceivePromotion { get; set; }

            [Display(Name = "Favourite Genre")]
            public int FavouriteGenre { get; set; }

            [Display(Name = "Favourite Platform")]
            public int FavouritePlatform { get; set; }
        }

        private async Task LoadAsync(CvgsUser user)
        {
            if (user != null)
            {
                Username = user.UserName;

                Input = new InputModel
                {
                    FavouriteGenre = user.FavouriteGenreId,
                    FavouritePlatform = user.FavouritePlatformId,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Gender = user.Gender,
                    DateOfBirth = user.DateOfBirth,
                    ReceivePromotion = user.ReceivePromotion,
                    MailingAddress = user.MailingAddress,
                    ShippingAddress = user.ShippingAddress
                };
            }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            ViewData["GenreId"] = new SelectList(_context.Set<CvgsGenre>(), "GenreId", "Type");
            ViewData["PlatformId"] = new SelectList(_context.Set<CvgsPlatform>(), "PlatformId", "Name");

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            //user.PhoneNumber = Input.PhoneNumber;
            user.FavouriteGenreId = Input.FavouriteGenre;
            user.FavouritePlatformId = Input.FavouritePlatform;
            user.FirstName = Input.FirstName;
            user.LastName = Input.LastName;
            user.Gender = Input.Gender;
            user.DateOfBirth = Input.DateOfBirth;
            user.ReceivePromotion = Input.ReceivePromotion;

            user.MailingAddress = Input.MailingAddress;
            user.ShippingAddress = Input.ShippingAddress;

            await _userManager.UpdateAsync(user);

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
