using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLySinhVien_BTL.Data;
using QuanLySinhVien_BTL.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace QuanLySinhVien_BTL.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TranscriptController : Controller
    {
        private readonly Data.ApplicationDbContext _context;

        public TranscriptController(Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string seachString, string CourseId)
        {
            ViewData["CurrentFilter"] = seachString;
            ViewData["CurrentCourseId"] = CourseId;
            ViewBag.Courses = new SelectList(_context.Courses.ToList(), "CourseId", "CourseName", CourseId);

            var transcripts = _context.Transcripts
                .Include(t => t.Student)
                .Include(t => t.Course)
                .AsQueryable();

            foreach (var t in transcripts)
            {
                if (t.Course != null)
                {
                    t.GPA = t.ProcessGrade * t.Course.Coefficient + t.FinalGrade * (1 - t.Course.Coefficient);
                }
            }

            if (!string.IsNullOrEmpty(seachString))
            {
                if (int.TryParse(seachString, out int TranscriptId))
                {
                    transcripts = transcripts.Where(t => t.TranscriptId == TranscriptId);
                }
                else
                {
                    transcripts = transcripts.Where(t => t.TranscriptId.ToString().Contains(seachString));
                }
            }

            if (!string.IsNullOrEmpty(CourseId))
            {
                if (int.TryParse(CourseId, out int courseIdValue))
                {
                    transcripts = transcripts.Where(t => t.CourseId == courseIdValue);
                }
            }

            return View(transcripts.ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Students = new SelectList(_context.Students.ToList(), "Id", "Name");
            ViewBag.Courses = new SelectList(_context.Courses.ToList(), "CourseId", "CourseName");
            return View(new Transcript());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Transcript model)
        {
            if (ModelState.IsValid)
            {

                var course = await _context.Courses.FindAsync(model.CourseId);

                if (course == null)
                {
                    Console.WriteLine("Null");
                }

                if (course != null)
                {
                    var gpa = model.ProcessGrade * course.Coefficient + model.FinalGrade * (1 - course.Coefficient);
                    Console.WriteLine($"GPA: {gpa}");
                }

                _context.Transcripts.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Students = new SelectList(_context.Students.ToList(), "Id", "Name", model.StudentId);
            ViewBag.Courses = new SelectList(_context.Courses.ToList(), "CourseId", "CourseName", model.CourseId);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var transcript = await _context.Transcripts.FindAsync(id);
            if (transcript == null)
            {
                return NotFound();
            }

            return View(transcript);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Transcript model)
        {
            if (ModelState.IsValid)
            {
                _context.Transcripts.Update(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        private bool TranscriptExists(int id)
        {
            return _context.Transcripts.Any(e => e.TranscriptId == id);
        }
    }
}
