using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using elearningapp.Data;
using elearningapp.Models;
using LearningApp.Models;
using Microsoft.AspNetCore.Authorization;
using PagedList;
using PagedList.Mvc;
using NuGet.Protocol.Core.Types;
using System.Drawing.Printing;

namespace LearningApp.Controllers
{
    public class CoursesController : Controller
    {
        private readonly LearningAppIdentityDbContext _context;

        public CoursesController(LearningAppIdentityDbContext context)
        {
            _context = context;
        }

		// GET: Courses
		public async Task<IActionResult> Index(int p = 1)
		{
		    var headings = _context.Courses.ToPagedList(p, 6);
			return _context.Courses != null ?
			View(headings) :
              Problem("Entity set 'LearningAppIdentityDbContext.Courses'  is null.");
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Courses == null)
            {
                return NotFound();
            }

            var courses = await _context.Courses
                .Include(c => c.Instructor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (courses == null)
            {
                return NotFound();
            }

            return View(courses);
        }

        // GET: Courses/Create
        public IActionResult Create()
        {
            var instructors = (from user in _context.Users
								join userRole in _context.UserRoles on user.Id equals userRole.UserId
								join role in _context.Roles on userRole.RoleId equals role.Id
								where role.Name == "Instructor"
							   select new SelectListItem
							   {
								   Value = user.Id,
								   Text = user.UserName,

							   }).ToList();

			ViewData["InstructorId"] = instructors;
            return View(new CourseDTO());
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,InstructorId,Category,EnrollmentCount,ImageUrl,CourseDuration")] CourseDTO courses)
        {
            if (ModelState.IsValid)
            {

                _context.Add(new Courses
                {
                    Title = courses.Title,
                    ImageUrl = courses.ImageUrl,
                    Description = courses.Description,
                    CourseDuration = courses.CourseDuration,
                    EnrollmentCount = courses.EnrollmentCount,
                    InstructorId = courses.InstructorId,
                    Category = courses.Category
                });
                await _context.SaveChangesAsync();
                return RedirectToAction("CourseList", "Users");
            }
            ViewData["InstructorId"] = new SelectList(_context.Users, "Id", "Id", courses.InstructorId);
            return View(courses);
        }
        [Authorize(Roles = "Admin, Instructor")]
        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var instructors = (from user in _context.Users
                               join userRole in _context.UserRoles on user.Id equals userRole.UserId
                               join role in _context.Roles on userRole.RoleId equals role.Id
                               where role.Name == "Instructor"
                               select new SelectListItem
                               {
                                   Value = user.Id,
                                   Text = user.UserName,

                               }).ToList();

            if (id == null || _context.Courses == null)
            {
                return NotFound();
            }

			var courses = await _context.Courses.FindAsync(id);
            if (courses == null)
            {
                return NotFound();
            }
            ViewData["InstructorData"] = instructors;
            return View(courses);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,InstructorId,Category,EnrollmentCount,ImageUrl,CourseDuration")] CourseDTO courses)
        {
            if (id != courses.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingCourse = await _context.Courses.FindAsync(id);
                    if (existingCourse == null)
                    {
                        return NotFound();
                    }

                    // Map data from CourseDTO to existingCourse
                    existingCourse.Title = courses.Title;
                    existingCourse.ImageUrl = courses.ImageUrl;
                    existingCourse.Description = courses.Description;
                    existingCourse.CourseDuration = courses.CourseDuration;
                    existingCourse.EnrollmentCount = courses.EnrollmentCount;
                    existingCourse.InstructorId = courses.InstructorId;
                    existingCourse.Category = courses.Category;

                    _context.Update(existingCourse);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CoursesExists(courses.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("CourseList","Users");
            }
            ViewData["InstructorId"] = new SelectList(_context.Users, "Id", "Id", courses.InstructorId);
            return View(courses);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Courses == null)
            {
                return NotFound();
            }

            var courses = await _context.Courses
                .Include(c => c.Instructor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (courses == null)
            {
                return NotFound();
            }

            return View(courses);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Courses == null)
            {
                return Problem("Entity set 'LearningAppIdentityDbContext.Courses'  is null.");
            }
            var courses = await _context.Courses.FindAsync(id);
            if (courses != null)
            {
                _context.Courses.Remove(courses);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction("CourseList", "Users");
        }

        private bool CoursesExists(int id)
        {
          return (_context.Courses?.Any(e => e.Id == id)).GetValueOrDefault();
        }
		[HttpPost]
		public async Task<IActionResult> Enroll(int courseId)
		{
			var course = await _context.Courses.FindAsync(courseId);
			var userId = _context.Users.SingleOrDefault(u => u.UserName == User.Identity.Name).Id;
			var courseID = _context.Courses.SingleOrDefault(c => c.Id == courseId).Id;

			var isEnrolled = await _context.Enrollments
			.AnyAsync(e => e.CourseId == courseId && e.UserId == userId);

			if (isEnrolled)
			{
				// Handle the case where the user is already enrolled
				// You can display a message or redirect to a specific page
				return Ok("Already enrolled");
			}

			var enrollment = new Enrollments
			{
				CourseId = courseID,
				UserId = userId,
				EnrollmentDate = DateTime.Now
			};


			_context.Enrollments.Add(enrollment);
			course.EnrollmentCount++;
			await _context.SaveChangesAsync();

			return Ok("The user has been enrolled in the course.");
		}
	}
}
