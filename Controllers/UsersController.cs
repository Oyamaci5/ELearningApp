using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using elearningapp.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using LearningApp.Controllers;

namespace elearningapp.Controllers
{
    [Authorize (Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        private readonly LearningAppDbContext _context;
        private readonly LearningAppIdentityDbContext _idcontext;

        public UsersController(UserManager<IdentityUser> userManager , LearningAppDbContext context, LearningAppIdentityDbContext contextid )
        {
            _context = context;
            _idcontext = contextid; 
			_userManager = userManager;
		}
        //GET : Dashboard
        public async Task<IActionResult> Dashboard()
        {
			return View();
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {

            var users = await _userManager.Users.ToListAsync();
            List<UserWasRole> list = new List<UserWasRole>();

            
            /*var roles = await _userManager.GetRolesAsync(user.Identity.GetUserId());  */
            foreach (IdentityUser user in users)
            {
                UserWasRole UserwithRole = new UserWasRole();
                UserwithRole.Roles = new List<string>();
                UserwithRole.Username = user.UserName;
                //UserwithRole.Roles = _userManager.GetRolesAsync(user.Id.ToString());     
                var sqlRoles = from x in _idcontext.UserRoles
                            where x.UserId == user.Id
                            select x;
                //UserwithRole.Roles = sqlRoles.ToList<string>();
                foreach(var role in sqlRoles)
                {
                    
					var roleNames = from x in _idcontext.Roles
								   where x.Id == role.RoleId
								   select x;
                    foreach(var name in roleNames)
                    {
						UserwithRole.Roles.Add(name.Name);
					}
				}
                list.Add(UserwithRole);
            }

            return View(list);

        }
        
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserName,Email,PasswordHash")] IdentityUser user)
        {
            if (ModelState.IsValid)
            {
                var result = await _userManager.CreateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(user);
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,UserName,Email")] IdentityUser user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByIdAsync(id);
                if (existingUser == null)
                {
                    return NotFound();
                }

                existingUser.UserName = user.UserName;
                existingUser.Email = user.Email;

                var result = await _userManager.UpdateAsync(existingUser);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return NotFound();
        }
    }


}

