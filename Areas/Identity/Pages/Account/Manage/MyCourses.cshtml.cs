// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using System;
using System.Threading.Tasks;
using elearningapp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace elearningapp.Areas.Identity.Pages.Account.Manage
{
    public class MyCoursesData : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly LearningAppIdentityDbContext _context;
        private readonly ILogger<MyCoursesData> _logger;

        public MyCoursesData(
            UserManager<IdentityUser> userManager,
            ILogger<MyCoursesData> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            var courses = _context.Courses.ToList();
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            ViewData["Courses"] = courses;
            return Page();
        }
    }
}
