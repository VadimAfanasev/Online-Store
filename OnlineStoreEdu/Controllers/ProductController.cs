using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineStoreEdu.Data;
using OnlineStoreEdu.Models;
using OnlineStoreEdu.Models.ViewModels;

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
            //IEnumerable<SelectListItem> CetegoryDropDown = _db.Category.Select(i => new SelectListItem
            //{
            //    Text = i.Name,
            //    Value = i.Id.ToString()
            //});

            ////ViewBag.CetegoryDropDown = CetegoryDropDown;
            //ViewData["CetegoryDropDown"] = CetegoryDropDown;

            //Product product = new Product();

            ProductVM ProductVM = new ProductVM()
            {
                Product = new Product(),
                CategorySelectList = _db.Category.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })

            };

            if (id == null)
            {
                //this is for create
                return View(ProductVM);
            }
            else
            {
                ProductVM.Product = _db.Product.Find(id);
                if (ProductVM.Product == null)
                {
                    return NotFound();
                }
			    return View(ProductVM);
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
