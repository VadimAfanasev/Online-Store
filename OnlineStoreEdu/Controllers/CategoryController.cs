using Microsoft.AspNetCore.Mvc;

namespace OnlineStoreEdu.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
