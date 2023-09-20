using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using elearningapp.Data;
using elearningapp.Models;

namespace elearningapp.Controllers
{
    public class AssignmentsController : Controller
    {
        private readonly LearningAppIdentityDbContext _context;

        public AssignmentsController(LearningAppIdentityDbContext context)
        {
            _context = context;
        }

        // GET: Assignments
        public async Task<IActionResult> Index(int id)
        {
            var con = (from x in _context.Courses
                     where x.Id == id
                     select x).FirstOrDefault();
            CourseDetailsViewModel concon = new CourseDetailsViewModel();

            concon.CourseTitle = con.Title;
            concon.CourseImageUrl = con.ImageUrl;
            concon.CourseDescription = con.Description;
            concon.CourseCategory = con.Category;
            List<Assignments> Assignments = new List<Assignments>();
            var assign = (from x in _context.Assignments
                          where x.CourseId == id
                          select x).ToList();
            Assignments = assign;
            return View(concon);
           
        }
        // GET: Assignments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Assignments == null)
            {
                return NotFound();
            }

            var assignments = await _context.Assignments
                .FirstOrDefaultAsync(m => m.Id == id);
            if (assignments == null)
            {
                return NotFound();
            }

            return View(assignments);
        }

        // GET: Assignments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Assignments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CourseId,Title,Description,DueDate")] Assignments assignments)
        {
            if (ModelState.IsValid)
            {
                _context.Add(assignments);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(assignments);
        }

        // GET: Assignments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Assignments == null)
            {
                return NotFound();
            }

            var assignments = await _context.Assignments.FindAsync(id);
            if (assignments == null)
            {
                return NotFound();
            }
            return View(assignments);
        }

        // POST: Assignments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CourseId,Title,Description,DueDate")] Assignments assignments)
        {
            if (id != assignments.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(assignments);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AssignmentsExists(assignments.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(assignments);
        }

        // GET: Assignments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Assignments == null)
            {
                return NotFound();
            }

            var assignments = await _context.Assignments
                .FirstOrDefaultAsync(m => m.Id == id);
            if (assignments == null)
            {
                return NotFound();
            }

            return View(assignments);
        }

        // POST: Assignments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Assignments == null)
            {
                return Problem("Entity set 'LearningAppDbContext.Assignments'  is null.");
            }
            var assignments = await _context.Assignments.FindAsync(id);
            if (assignments != null)
            {
                _context.Assignments.Remove(assignments);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AssignmentsExists(int id)
        {
          return (_context.Assignments?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
