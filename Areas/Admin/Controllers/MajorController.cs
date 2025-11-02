using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuanLySinhVien_BTL.Data;
using QuanLySinhVien_BTL.Models;

namespace QuanLySinhVien_BTL.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MajorController : Controller
    {
        private readonly ApplicationDbContext _context;
        public MajorController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string searchString, string MajorId)
        {
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentMajorId"] = MajorId;

            var majors = _context.Majors.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                majors = majors.Where(m => m.MajorId.Contains(searchString) || m.Name.Contains(searchString));
            }
            if (!string.IsNullOrEmpty(MajorId))
            {
                majors = majors.Where(m => m.MajorId == MajorId);
            }
            return View(majors.ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Departments = new SelectList(_context.Departments.ToList(), "DepartmentId", "Name");
            return View(new Major());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Major model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Departments = new SelectList(_context.Departments.ToList(), "DepartmentId", "Name", model.DepartmentId);
                return View(model);
            }
            _context.Majors.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            var major = await _context.Majors.FindAsync(id);
            if (major == null)
            {
                return NotFound();
            }
            ViewBag.Departments = new SelectList(_context.Departments.ToList(), "DepartmentId", "Name", major.DepartmentId);
            return View(major);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Major model)
        {
            if (ModelState.IsValid)
            {
                _context.Majors.Update(model);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string? id)
        {
            var major = await _context.Majors.FindAsync(id);
            if (major == null)
            {
                return NotFound();
            }
            _context.Majors.Remove(major);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
