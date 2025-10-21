using Microsoft.AspNetCore.Mvc;

namespace QuanLySinhVien_BTL.Areas.Admin.Controllers
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
