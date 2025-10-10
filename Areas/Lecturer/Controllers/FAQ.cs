using Microsoft.AspNetCore.Mvc;

namespace QuanLySinhVien_BTL.Areas.Lecturer.Controllers
{
    public class FAQ : Controller
    {
        public class FAQController : Controller
        {
            [Area("Lecturer")]
            public IActionResult Index()
            {
                return View();
            }
        }
    }
}
