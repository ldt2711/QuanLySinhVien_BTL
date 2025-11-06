using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLySinhVien_BTL.Data;
using QuanLySinhVien_BTL.Models;

namespace QuanLySinhVien_BTL.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CourseController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CourseController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string searchString, string MajorId)
        {
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentMajorId"] = MajorId;
            ViewBag.Majors = new SelectList(_context.Majors.ToList(), "MajorId", "Name", MajorId);

            var courses = _context.Courses.Include(c => c.Major).Include(c => c.Lecturer).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                if (int.TryParse(searchString, out int courseId))
                {
                    courses = courses.Where(c => c.CourseId == courseId || c.CourseName.Contains(searchString));
                }
                else
                {
                    courses = courses.Where(c => c.CourseName.Contains(searchString));
                }
            }

            if (!string.IsNullOrEmpty(MajorId))
            {
                courses = courses.Where(c => c.MajorId == MajorId);
            }

            return View(courses.ToList());

        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Majors = new SelectList(_context.Majors.ToList(), "MajorId", "Name");
            ViewBag.Lecturers = new SelectList(_context.Lecturers.ToList(), "Id", "Name");
            return View(new Course());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Course model)
        {
            if (model.LecturerId == 0)
            {
                ModelState.AddModelError("LecturerId", "Phải chọn giảng viên.");
            }
            if (!ModelState.IsValid)
            {
                ViewBag.Majors = new SelectList(_context.Majors.ToList(), "MajorId", "Name", model.MajorId);
                ViewBag.Lecturers = new SelectList(_context.Lecturers.ToList(), "Id", "Name", model.LecturerId);
                return View(model);
            }
            _context.Courses.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            ViewBag.Majors = new SelectList(_context.Majors.ToList(), "MajorId", "Name", course.MajorId);
            ViewBag.Lecturers = new SelectList(_context.Lecturers.ToList(), "Id", "Name", course.LecturerId);
            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Course model)
        {
            if (model.LecturerId == 0)
            {
                ModelState.AddModelError("LecturerId", "Phải chọn giảng viên.");
            }
            if (id != model.CourseId)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                ViewBag.Majors = new SelectList(_context.Majors.ToList(), "MajorId", "Name", model.MajorId);
                ViewBag.Lecturers = new SelectList(_context.Lecturers.ToList(), "Id", "Name", model.LecturerId);
                return View(model);
            }
            _context.Courses.Update(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.CourseId == id);
        }
    }
}
