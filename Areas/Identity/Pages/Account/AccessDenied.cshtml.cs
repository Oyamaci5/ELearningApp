// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// "Nullable reference types" will not be used in this code block
#nullable disable

using Microsoft.AspNetCore.Mvc.RazorPages;

namespace elearningapp.Areas.Identity.Pages.Account
{
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    public class AccessDeniedModel : PageModel
    {
        /// <summary>
        /// This method handles HTTP GET requests to the Access Denied page.
        /// </summary>
        public void OnGet()
        {
            // This method currently doesn't have any specific functionality.
            // It is intended to respond to GET requests for the Access Denied page.
        }
    }
}