using Microsoft.AspNetCore.Mvc;

namespace QuanLySinhVien_BTL.Controllers
{
    public class StudentController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
    }
}
