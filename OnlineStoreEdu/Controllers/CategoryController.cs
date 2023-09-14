using Microsoft.AspNetCore.Mvc;
using OnlineStoreEdu.Data;
using OnlineStoreEdu.Models;

namespace OnlineStoreEdu.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Category> objList = _db.Category;
            return View(objList);
        }
    }
}
