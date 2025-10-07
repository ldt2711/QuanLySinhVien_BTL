using Microsoft.AspNetCore.Mvc;

namespace QuanLySinhVien_BTL.Controllers
{
    public class StudentController : Controller
    {
        public StudentController()
        {
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
