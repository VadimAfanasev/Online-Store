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

        //GET - Create
		public IActionResult Create()
		{
			return View();
		}

        //POST - Create
        [HttpPost]
        [ValidateAntiForgeryToken]
		public IActionResult Create(Category obj)
		{
            if (ModelState.IsValid) 
            {
                _db.Category.Add(obj);
                _db.SaveChanges();
			    return RedirectToAction("Index");
            }
            return View(); 
		}
	}
}
