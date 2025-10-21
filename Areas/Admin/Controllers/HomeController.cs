using Microsoft.AspNetCore.Mvc;

namespace QuanLySinhVien_BTL.Area.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
