using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<ApplicationUser> _userManager;

        public StudentController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index(string searchString, string MajorId)
        {
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentMajorId"] = MajorId;
            ViewBag.Majors = new SelectList(_context.Majors.ToList(), "MajorId", "Name", MajorId);

            var students = _context.Students
                .Include(s => s.Major)
                .AsQueryable();

            // Tìm kiếm theo ID hoặc Tên
            if (!string.IsNullOrEmpty(searchString))
            {
                if (int.TryParse(searchString, out int studentId))
                {
                    students = students.Where(s => s.Id == studentId || s.Name.Contains(searchString));
                }
                else
                {
                    students = students.Where(s => s.Name.Contains(searchString));
                }
            }

            // Lọc theo Ngành
            if (!string.IsNullOrEmpty(MajorId))
            {
                students = students.Where(s => s.MajorId == MajorId);
            }

            return View(students.ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Majors = new SelectList(_context.Majors.ToList(), "MajorId", "Name");
            return View(new Models.Student());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Models.Student model)
        {
            if (ModelState.IsValid)
            {
                // 1. Lưu student tạm để có Id
                _context.Add(model);
                await _context.SaveChangesAsync();

                // 2. Tạo user tự động dựa vào email
                var email = model.Email;
                var name = $"{model.Name}_{Guid.NewGuid().ToString().Substring(0, 5)}";
                var password = email.Split('@')[0];

                var user = new ApplicationUser
                {
                    UserName = name,
                    Email = email
                };

                var result = await _userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    // 3. Gán UserId cho sinh viên
                    model.UserId = user.Id;
                    _context.Update(model);
                    await _context.SaveChangesAsync();

                    // 4. Thêm role SinhVien
                    await _userManager.AddToRoleAsync(user, "Sinh Viên");

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // Nếu lỗi, có thể log ra
                    ModelState.AddModelError("", string.Join(";", result.Errors.Select(e => e.Description)));
                }
            }
            ViewBag.Majors = new SelectList(_context.Majors.ToList(), "MajorId", "Name", model.MajorId);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var student = await _context.Students.FindAsync(id);
            if (student == null)
                return NotFound();

            ViewBag.Majors = new SelectList(_context.Majors.ToList(), "MajorId", "Name", student.MajorId);
            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,DateOfBirth,Gender,Phone,Address,Email,MajorId")] Models.Student student)
        {
            if (id != student.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Majors = new SelectList(_context.Majors.ToList(), "MajorId", "Name", student.MajorId);
            return View(student);
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(student.UserId))
            {
                var user = await _userManager.FindByIdAsync(student.UserId);
                if (user != null)
                {
                    await _userManager.DeleteAsync(user);
                }
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
