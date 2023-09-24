using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        //GET - UPSERT
		public IActionResult Upsert(int? id)
		{
            IEnumerable<SelectListItem> CetgotyDropDown = _db.Category.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });

            ViewBag.CetgotyDropDown = CetgotyDropDown;

            Product product = new Product();
            if (id == null)
            {
                //this is for create
                return View(product);
            }
            else
            {
                product = _db.Product.Find(id);
                if (product == null)
                {
                    return NotFound();
                }
			    return View(product);
            }
		}

        //POST - UPSERT
        [HttpPost]
        [ValidateAntiForgeryToken]
		public IActionResult Upsert(Product obj)
		{
            if (ModelState.IsValid) 
            {
                _db.Product.Add(obj);
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
