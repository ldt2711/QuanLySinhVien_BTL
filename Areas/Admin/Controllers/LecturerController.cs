using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLySinhVien_BTL.Data;
using QuanLySinhVien_BTL.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace QuanLySinhVien_BTL.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class LecturerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LecturerController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString, string DepartmentCode)
        {
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentDepartmentId"] = DepartmentCode;

            ViewBag.Departments = new SelectList(_context.Departments.ToList(), "Id", "Name", DepartmentCode);
            var lecturers = _context.Lecturers
                                    .Include(l => l.Department) 
                                    .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                if (int.TryParse(searchString, out int lecturerId))
                {
                    lecturers = lecturers.Where(l => l.Id == lecturerId || l.Name.Contains(searchString));
                }
                else
                {
                    lecturers = lecturers.Where(l => l.Name.Contains(searchString));
                }
            }

            // Lọc theo Khoa
            if (!string.IsNullOrEmpty(DepartmentCode))
            {
                lecturers = lecturers.Where(l => l.DepartmentCode == DepartmentCode);
            }  

            return View(await lecturers.ToListAsync());
        }

        //public IActionResult Index()
        //{
        //    var lecturers = _context.Lecturers
        //        .Include(l => l.Department)
        //        .ToList();
        //    return View(lecturers);
        //}

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Departments = new SelectList(_context.Departments.ToList(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Lecturer model)
        {
            Console.WriteLine($"➡ DepartmentCode nhận được: {model.DepartmentCode}");
            if (!ModelState.IsValid)
            {
                ViewBag.Departments = new SelectList(_context.Departments.ToList(), "Id", "Name", model.DepartmentCode);
                return View(model);
            }
            _context.Lecturers.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var lecturer = _context.Lecturers.Find(id);
            if (lecturer == null)
                return NotFound();
            _context.Lecturers.Remove(lecturer);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lecturer = await _context.Lecturers.FindAsync(id);
            if (lecturer == null)
            {
                return NotFound();
            }

            ViewBag.Departments = new SelectList(_context.Departments.ToList(), "Id", "Name", lecturer.DepartmentCode);
            return View(lecturer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // 🎯 Cập nhật [Bind] để chỉ bao gồm các thuộc tính có thể chỉnh sửa 🎯
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Gender,Email,PhoneNumber,DepartmentId")] Lecturer lecturer)
        {
            if (id != lecturer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid) // ModelState.IsValid sẽ luôn true nếu không có DataAnnotations hay lỗi kiểu dữ liệu
            {
                try
                {
                    _context.Update(lecturer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LecturerExists(lecturer.Id))
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
            ViewBag.Departments = new SelectList(_context.Departments.ToList(), "Id", "Name", lecturer.DepartmentCode);
            return View(lecturer);
        }

        private bool LecturerExists(int id)
        {
            return _context.Lecturers.Any(e => e.Id == id);
        }
    }
}
