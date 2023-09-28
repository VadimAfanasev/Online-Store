using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineStoreEdu.Data;
using OnlineStoreEdu.Models;
using OnlineStoreEdu.Models.ViewModels;

namespace OnlineStoreEdu.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
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
		public IActionResult Upsert(ProductVM productVM)
		{
            if (ModelState.IsValid) 
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;

                if (productVM.Product.Id == 0)
                {
                    //Creating
                    string upload = webRootPath + WC.ImagePath;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);

                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    productVM.Product.Image = fileName + extension;

                    _db.Product.Add(productVM.Product);
                }
                else
                {
                    //Updating
                    var objFromDb = _db.Product.AsNoTracking().FirstOrDefault(c => c.Id == productVM.Product.Id);

                    if (files.Count > 0)
                    {
                        string upload = webRootPath + WC.ImagePath;
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);

                        var oldFile = Path.Combine(upload, objFromDb.Image);

                        if (System.IO.File.Exists(oldFile))
                        {
                            System.IO.File.Delete(oldFile);
                        }

                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }

                        productVM.Product.Image = fileName + extension;
                    }
                    else
                    {
                        productVM.Product.Image = objFromDb.Image;
                    }

                    _db.Product.Update(productVM.Product);
                }


                _db.SaveChanges();
			    return RedirectToAction("Index");
            }
            productVM.CategorySelectList = _db.Category.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });

            return View(productVM); 
		}

        //GET - DELETE
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Product product = _db.Product.Include(c => c.Category).FirstOrDefault(x => x.Id == id);
            //product.Category = _db.Category.Find(product.CategoryId);
             
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        //POST - DELETE
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _db.Product.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            string upload = _webHostEnvironment.WebRootPath + WC.ImagePath;

            var oldFile = Path.Combine(upload, obj.Image);

            if (System.IO.File.Exists(oldFile))
            {
                System.IO.File.Delete(oldFile); 
            }

            _db.Product.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
