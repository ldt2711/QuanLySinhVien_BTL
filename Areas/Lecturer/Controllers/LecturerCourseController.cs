using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLySinhVien_BTL.Data;
using QuanLySinhVien_BTL.Models;
using QuanLySinhVien_BTL.ViewModels;

namespace QuanLySinhVien_BTL.Areas.Lecturer.Controllers
{
    [Area("Lecturer")]
    [Authorize(Roles = "Giảng Viên")]
    public class LecturerCourseController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public LecturerCourseController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /Lecturer/LecturerCourse/
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            // Lấy giảng viên hiện tại
            var lecturer = await _context.Lecturers
                .FirstOrDefaultAsync(l => l.UserId == user.Id);

            if (lecturer == null) return NotFound("Không tìm thấy giảng viên.");

            // Lấy danh sách lớp học phần giảng viên đang dạy
            var courses = await _context.Courses
                .Include(c => c.Major)
                .Include(c => c.Transcripts)
                .Where(c => c.LecturerId == lecturer.Id)
                .Select(c => new
                {
                    c.CourseId,
                    c.CourseName,
                    MajorName = c.Major.Name,
                    StudentCount = c.Transcripts.Count()
                })
                .ToListAsync();

            return View(courses);
        }

        // GET: /Lecturer/LecturerCourse/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var course = await _context.Courses
                .Include(c => c.Major)
                .FirstOrDefaultAsync(c => c.CourseId == id);

            if (course == null)
                return NotFound();

            // Lấy danh sách sinh viên trong lớp học phần
            var transcripts = await _context.Transcripts
                .Include(t => t.Student)
                .Include(t => t.Course)
                .Where(t => t.CourseId == id)
                .Select(t => new StudentTranscriptViewModel
                {
                    StudentId = t.StudentId,
                    StudentName = t.Student.Name,
                    ProcessGrade = t.ProcessGrade ?? 0,
                    FinalGrade = t.FinalGrade ?? 0,
                    GPA = (t.ProcessGrade ?? 0) * (t.Course.Coefficient) + (t.FinalGrade ?? 0) * (1 - t.Course.Coefficient)
                })
                .ToListAsync();

            // Truyền thông tin học phần qua ViewBag
            ViewBag.Course = course;

            // Trả về view danh sách sinh viên
            return View(transcripts);
        }

        // GET: /Lecturer/LecturerCourse/EditStudentGrade?studentId=1&courseId=2
        [HttpGet]
        public async Task<IActionResult> EditStudentGrade(int studentId, int courseId)
        {
            var transcript = await _context.Transcripts
                .Include(t => t.Student)
                .Include(t => t.Course)
                .FirstOrDefaultAsync(t => t.StudentId == studentId && t.CourseId == courseId);

            if (transcript == null)
                return NotFound();

            var model = new StudentTranscriptViewModel
            {
                StudentId = transcript.StudentId,
                StudentName = transcript.Student.Name,
                ProcessGrade = transcript.ProcessGrade ?? 0,
                FinalGrade = transcript.FinalGrade ?? 0,
                GPA = (transcript.ProcessGrade ?? 0) * transcript.Course.Coefficient
                    + (transcript.FinalGrade ?? 0) * (1 - transcript.Course.Coefficient)
            };

            ViewBag.Course = transcript.Course;
            return View(model);
        }

        // POST: /Lecturer/LecturerCourse/EditStudentGrade
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditStudentGrade(int studentId, int courseId, StudentTranscriptViewModel model)
        {
            var transcript = await _context.Transcripts
                .Include(t => t.Course)
                .FirstOrDefaultAsync(t => t.StudentId == studentId && t.CourseId == courseId);

            if (transcript == null)
                return NotFound();

            // Cập nhật điểm
            transcript.ProcessGrade = model.ProcessGrade;
            transcript.FinalGrade = model.FinalGrade;

            await _context.SaveChangesAsync();

            // Sau khi lưu, quay lại trang danh sách sinh viên
            return RedirectToAction("Details", new { id = courseId });
        }
    }
}
