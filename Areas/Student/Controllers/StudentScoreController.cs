using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLySinhVien_BTL.Data;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLySinhVien_BTL.Areas.Student.Controllers
{
    [Area("Student")]
    public class StudentScoreController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentScoreController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Hiển thị danh sách lớp học phần mà sinh viên đang học
        public async Task<IActionResult> Index()
        {
            // Giả sử ta lấy StudentId từ tài khoản đang đăng nhập
            var email = User.Identity?.Name;
            var student = await _context.Students.FirstOrDefaultAsync(s => s.Email == email);

            if (student == null)
                return NotFound("Không tìm thấy sinh viên.");

            var transcripts = await _context.Transcripts
                .Include(t => t.Course)
                .Include(t => t.Course.Lecturer)
                .Where(t => t.StudentId == student.Id)
                .ToListAsync();

            return View(transcripts);
        }

        // Trả về chi tiết điểm cho modal
        public async Task<IActionResult> Details(int id)
        {
            var transcript = await _context.Transcripts
                .Include(t => t.Course)
                .FirstOrDefaultAsync(t => t.TranscriptId == id);

            if (transcript == null)
                return NotFound();

            double? gpa = null;
            if (transcript.ProcessGrade.HasValue && transcript.FinalGrade.HasValue)
                gpa = transcript.ProcessGrade.Value * transcript.Course.Coefficient + transcript.FinalGrade.Value * (1 - transcript.Course.Coefficient);

            var data = new
            {
                courseName = transcript.Course.CourseName,
                processGrade = transcript.ProcessGrade,
                finalGrade = transcript.FinalGrade,
                gpa = gpa
            };

            return Json(data);
        }
    }
}
