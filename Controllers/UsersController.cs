using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using elearningapp.Data;
using LearningApp.Controllers;
using Microsoft.AspNetCore.Mvc.Rendering;
using LearningApp.Models;
using elearningapp.Models;

namespace elearningapp.Controllers
{

    public class UsersController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly LearningAppIdentityDbContext _idcontext;

        public UsersController(UserManager<IdentityUser> userManager, LearningAppIdentityDbContext contextid)
        {
            _idcontext = contextid;
            _userManager = userManager;
        }
        //GET : Dashboard
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> Dashboard()
        {
            var courses = (from x in _idcontext.Courses
                          select x).ToList();

            var courseCount = courses.Count();

            var instructorCount = (from user in _idcontext.Users
                                   join userRole in _idcontext.UserRoles on user.Id equals userRole.UserId
                                   join role in _idcontext.Roles on userRole.RoleId equals role.Id
                                   where role.Name == "Instructor"
                                   select user).ToList().Count();

            var studentCount = (from user in _idcontext.Users
                                 join userRole in _idcontext.UserRoles on user.Id equals userRole.UserId
                                 join role in _idcontext.Roles on userRole.RoleId equals role.Id
                                 where role.Name == "Student"
                                 select user).ToList().Count();

            var assignmentCount = (from assign in _idcontext.Assignments
                                   select assign).ToList().Count();

            var Dashboardmodel = new DashboardModel
            {
                AssignmentCount = assignmentCount,
                StudentCount = studentCount,
                CourseCount = courseCount,
                InstructorCount = instructorCount,
                courses = courses,

            };
            return View(Dashboardmodel);
        }
        [Authorize(Roles = "Admin")]
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
                UserwithRole.Id = user.Id;
                //UserwithRole.Roles = _userManager.GetRolesAsync(user.Id.ToString());     
                var sqlRoles = (from x in _idcontext.UserRoles
                                where x.UserId == user.Id
                                select x).ToList();
                //UserwithRole.Roles = sqlRoles.ToList<string>();
                foreach (var role in sqlRoles)
                {

                    var roleNames = (from x in _idcontext.Roles
                                     where x.Id == role.RoleId
                                     select x).ToList();
                    foreach (var name in roleNames)
                    {
                        UserwithRole.Roles.Add(name.Name);
                    }
                }
                list.Add(UserwithRole);
            }

            return View(list);

        }
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);

            UserWasRole UserwithRole = new UserWasRole();

            UserwithRole.Username = user.UserName;
            UserwithRole.Id = user.Id;

            var roleNames = (from x in _idcontext.UserRoles
                             join y in _idcontext.Roles on x.RoleId equals y.Id
                             where x.UserId == user.Id
                             select y.Name);
            UserwithRole.RoleName = roleNames.FirstOrDefault();
            UserwithRole.Email = user.Email;
            if (user == null)
            {
                return NotFound();
            }

            return View(UserwithRole);
        }
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
                    return RedirectToAction("", "Users");
                }
            }

            return NotFound();
        }
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> CourseList()
        {

            List<CourseDTO> courses = new List<CourseDTO>();
            foreach (var item in _idcontext.Courses)
            {
                var kapali = (from user in _idcontext.Users
                              where user.Id == item.InstructorId
                              select user.UserName
                    ).ToList();

                var courseDTO = new CourseDTO
                {
                    Id = item.Id,
                    // Populate other properties as needed
                    InstructorName = kapali.FirstOrDefault(),
                    Title = item.Title,
                    Description = item.Description,
                    Category = item.Category,
                    InstructorId = item.InstructorId,
                    EnrollmentCount = item.EnrollmentCount,
                    CourseDuration = item.CourseDuration, 
                    ImageUrl = item.ImageUrl,

                };
                courses.Add(courseDTO);


            }
            return _idcontext.Courses != null ?
                          View(courses) :
                          Problem("Entity set 'LearningAppDbContext.Courses'  is null.");
        }

        [HttpGet]
        public async Task<IActionResult> AssignmentList([FromQuery] int CourseId)
        {
            List<Assignments> assignments = new List<Assignments>((from x in _idcontext.Assignments
                                                                   where x.CourseId == CourseId
																   select x
                        ).ToList());

            ViewData["CourseName"] = (from x in _idcontext.Courses
                                     where x.Id == CourseId
									  select x.Title).FirstOrDefault();
            ViewData["CourseId"] = CourseId;
            return View(assignments);
        }

    }
}

