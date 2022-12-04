// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using CVGS_Main.Areas.Identity.Data;
using CVGS_Main.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CVGS_Main.Areas.Identity.Pages.Account.Manage
{
    public class PaymentMethodsModel : PageModel
    {
        private readonly UserManager<CvgsUser> _userManager;
        private readonly SignInManager<CvgsUser> _signInManager;
        private readonly ILogger<PaymentMethodsModel> _logger;
        private readonly CvgsDbContext _context;

        public PaymentMethodsModel(
            UserManager<CvgsUser> userManager,
            SignInManager<CvgsUser> signInManager,
            ILogger<PaymentMethodsModel> logger,
            CvgsDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [Display(Name = "Name On Card")]
            public string NameOnCard { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [Display(Name = "Card Number")]
            public string CardNumber { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [Display(Name = "Security Code")]
            public string SecurityCode { get; set; }

            [Required]
            [Display(Name = "Expiry Date")]
            public DateTime ExpiryDate { get; set; }

            public IEnumerable<CvgsPaymentMethod> PaymentMethods { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var paymentMethods = _context.CvgsPaymentMethod.
                Where(p => p.UserId == user.Id).ToList();
            
            
            if (paymentMethods != null)
            {
                Input = new InputModel
                {
                    PaymentMethods = paymentMethods
                };
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            CvgsPaymentMethod paymentMethod = new CvgsPaymentMethod
            {
                NameOnCard = Input.NameOnCard,
                CardNumber = Input.CardNumber,
                SecurityCode = Input.SecurityCode,
                ExpiryDate = Input.ExpiryDate,
                UserId = user.Id
            };

            await _context.CvgsPaymentMethod.AddAsync(paymentMethod);

            await _userManager.UpdateAsync(user);
            await _signInManager.RefreshSignInAsync(user);

            StatusMessage = "Added payment method to your account!";

            return RedirectToPage();
        }
    }
}
