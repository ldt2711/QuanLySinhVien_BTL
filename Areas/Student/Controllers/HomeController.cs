using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace QuanLySinhVien_BTL.Areas.Student.Controllers
{
    [Area("Student")]
    [Authorize(Roles = "SinhVien")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}