using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace QuanLySinhVien_BTL.Areas.Lecturer.Controllers
{
    [Area("Lecturer")]
    [Authorize(Roles = "GiangVien")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}