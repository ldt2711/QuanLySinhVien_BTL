using Microsoft.AspNetCore.Mvc;

namespace QuanLySinhVien_BTL.Area.Lecturer.Controllers
{
    [Area("Lecturer")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
