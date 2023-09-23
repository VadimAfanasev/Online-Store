using Microsoft.AspNetCore.Mvc;
using OnlineStoreEdu.Data;
using OnlineStoreEdu.Models;

namespace OnlineStoreEdu.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ProductController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> objList = _db.Product;
            foreach (var obj in objList)
            {
                obj.Category = _db.Category.FirstOrDefault(n => n.Id == obj.CategoryId);
            }

            return View(objList);
        }

        //GET - CREATE
		public IActionResult Create()
		{
			return View();
		}

        //POST - CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
		public IActionResult Create(Product obj)
		{
            if (ModelState.IsValid) 
            {
                _db.Product.Add(obj);
                _db.SaveChanges();
			    return RedirectToAction("Index");
            }
            return View(); 
		}

		//GET - EDIT
		public IActionResult Edit(int? id)
		{
            if (id == null || id ==0)
            {
                return NotFound();
            }
            var obj = _db.Product.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

			return View(obj);
		}

        //POST - EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product obj)
        {
            if (ModelState.IsValid)
            {
                _db.Product.Update(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        //GET - DELETE
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = _db.Product.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        //POST - DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _db.Product.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            _db.Product.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
