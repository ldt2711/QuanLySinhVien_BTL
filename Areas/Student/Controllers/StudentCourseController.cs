using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLySinhVien_BTL.Data;
using System.Security.Claims;


namespace QuanLySinhVien_BTL.Areas.Student.Controllers
{
    [Area("Student")]
    public class StudentCourseController : Controller
    {
        private readonly ApplicationDbContext _context;
        public StudentCourseController(ApplicationDbContext context)
        {
            _context = context;
        }
    }
}
