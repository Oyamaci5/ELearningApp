// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace elearningapp.Areas.Identity.Pages.Account
{
    // The ConfirmEmailModel class is the ASP.NET Core Razor Pages model that manages the functionality of the page.
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        // The constructor of the ConfirmEmailModel class. It takes an object of type UserManager<IdentityUser>.
        public ConfirmEmailModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        // The TempData property is used to store temporary data, in this case, it holds the status message of the page.
        [TempData]
        public string StatusMessage { get; set; }

        // The OnGetAsync method is executed when a GET request is made to the page.
        public async Task<IActionResult> OnGetAsync(string userId, string code)
        {
            if (userId == null || code == null)
            {
                // If userId or code is null, the user is redirected to the /Index page.
                return RedirectToPage("/Index");
            }

            // The user is retrieved based on their ID.
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                // If the user cannot be found, an error is returned.
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            // The confirmation code is obtained by decoding the Base64 URL code.
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));

            // The user's email is confirmed.
            var result = await _userManager.ConfirmEmailAsync(user, code);

            // If the confirmation is successful, set the success message; otherwise, set the error message.
            StatusMessage = result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.";

            // Finally, return the page.
            return Page();
        }
    }
}
