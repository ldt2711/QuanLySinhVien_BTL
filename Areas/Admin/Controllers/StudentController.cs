using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLySinhVien_BTL.Data;
using QuanLySinhVien_BTL.Models;

namespace QuanLySinhVien_BTL.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string searchString, int? majorId)
        {
            ViewData["CurrentFilter"] = searchString; // Giữ lại giá trị tìm kiếm
            ViewBag.Majors = new SelectList(_context.Majors.ToList(), "Id", "Name", majorId);

            var students = _context.Students
                .Include(s => s.Major)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                // Thử chuyển searchString sang số để tìm kiếm theo Id
                if (int.TryParse(searchString, out int studentId))
                {
                    students = students.Where(s => s.Id == studentId || s.Name.Contains(searchString));
                }
                else
                {
                    // Nếu không phải số, tìm kiếm theo tên
                    students = students.Where(s => s.Name.Contains(searchString));
                }
            }

            // Lọc theo chuyên ngành
            if (majorId.HasValue && majorId.Value > 0)
            {
                students = students.Where(s => s.MajorId == majorId.Value);
            }

            return View(students.ToList());
        }

        //public IActionResult Index()
        //{
        //    var students = _context.Students
        //        .Include(s => s.Major)
        //        .ToList();
        //    return View(students);
        //}

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Majors = new SelectList(_context.Majors.ToList(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student model)
        {
            Console.WriteLine($"➡ MajorId nhận được: {model.MajorId}");
            if (!ModelState.IsValid)
            {
                ViewBag.Majors = new SelectList(_context.Majors.ToList(), "Id", "Name", model.MajorId);
                return View(model);
            }

            _context.Students.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var student = _context.Students.Find(id);
            if (student == null)
                return NotFound();

            _context.Students.Remove(student);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
