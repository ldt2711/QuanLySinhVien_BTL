using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLySinhVien_BTL.Data;
using QuanLySinhVien_BTL.Models;
using System.Threading.Tasks;

namespace QuanLySinhVien_BTL.Controllers
{
    [Area("Student")]
    [Authorize(Roles = "Sinh Viên")]
    public class StudentCourseController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public StudentCourseController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Hiển thị danh sách các học phần có thể đăng ký
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            var student = await _context.Students
                .Include(s => s.Transcripts)
                .FirstOrDefaultAsync(s => s.UserId == user.Id);

            if (student == null)
                return NotFound("Không tìm thấy thông tin sinh viên.");

            // Danh sách học phần đã đăng ký
            var registeredCourseIds = student.Transcripts?.Select(t => t.CourseId).ToList() ?? new List<int>();
            var registeredCourses = await _context.Courses
                .Include(c => c.Lecturer)
                .Include(c => c.Major)
                .Where(c => registeredCourseIds.Contains(c.CourseId))
                .ToListAsync();

            // Danh sách học phần có thể đăng ký
            var availableCourses = await _context.Courses
                .Include(c => c.Lecturer)
                .Include(c => c.Major)
                .Where(c => !registeredCourseIds.Contains(c.CourseId))
                .ToListAsync();

            ViewBag.RegisteredCourses = registeredCourses;

            return View(availableCourses);
        }

        // Đăng ký học phần
        [HttpPost]
        public async Task<IActionResult> Register(int courseId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var student = await _context.Students.FirstOrDefaultAsync(s => s.UserId == user.Id);
            if (student == null) return NotFound("Không tìm thấy sinh viên.");

            bool exists = await _context.Transcripts.AnyAsync(t => t.StudentId == student.Id && t.CourseId == courseId);
            if (exists)
            {
                TempData["Message"] = "Bạn đã đăng ký học phần này rồi.";
                return RedirectToAction(nameof(Index));
            }

            var transcript = new Transcript
            {
                StudentId = student.Id,
                CourseId = courseId,
                ProcessGrade = 0,
                FinalGrade = 0
            };

            _context.Transcripts.Add(transcript);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Đăng ký học phần thành công!";
            return RedirectToAction(nameof(Index));
        }

        // Hủy đăng ký học phần
        [HttpPost]
        public async Task<IActionResult> Unregister(int courseId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var student = await _context.Students.FirstOrDefaultAsync(s => s.UserId == user.Id);
            if (student == null) return NotFound("Không tìm thấy sinh viên.");

            var transcript = await _context.Transcripts
                .FirstOrDefaultAsync(t => t.StudentId == student.Id && t.CourseId == courseId);

            if (transcript != null)
            {
                _context.Transcripts.Remove(transcript);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Hủy đăng ký học phần thành công!";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
