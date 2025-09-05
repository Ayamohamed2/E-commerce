using E_commerce.Data;
using E_commerce.Dataaccess.Reposatory;
using E_commerce.Dataaccess.Reposatory.IReposatory;
using E_commerce.Model.Models;
using E_commerce.Utility;
using E_commerce_project.Migrations;
using E_commerce_project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Immutable;
using System.Security.Policy;

namespace E_commerce_project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork unit;
        private readonly IWebHostEnvironment env;
        public ProductController(IUnitOfWork unit,IWebHostEnvironment env)
        {
            this.unit = unit;
            this.env = env;
        }
        public IActionResult Index()
        {
            List<Product> products = unit.Product.GetALL(Includes: "Category");
            return View(products);
        }
        [HttpGet]
        public IActionResult Create()
        {
            var categories = unit.Category.GetALL();
            ViewBag.categories = categories;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product Model)
        {
            var categories = unit.Category.GetALL();
            ViewBag.categories = categories;
            if (ModelState.IsValid)
            {

                List<ProductImage> images=new List<ProductImage>();

                foreach (var ImageFile in Model.files)
                {

                 
                    if (ImageFile != null)
                    {

                        ProductImage image = new ProductImage
                        {
                            ImageURL = GetImageURL(ImageFile, Model.Category_id),
                            Product_Id=Model.Id

                        };

                        images.Add(image);

                       
                    }

                }
                try
                {

                    Model.ProductImages = images;
                    unit.Product.Create(Model);
                    unit.save();
                    TempData["success"] = "Product created syccesfully";
                    return RedirectToAction("index");
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError("ImageFile", "Image is required");

                }
            }
           
            return View(Model);
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            var categories = unit.Category.GetALL();
            ViewBag.categories = categories;
            if (id == 0 || id == null) return NotFound();
            Product? product = unit.Product.GetByFilter(p => p.Id == id,Includes: "ProductImages");
            if (product == null) return NotFound();
            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int? id,Product model, List<IFormFile> NewImages)
        {
            var categories = unit.Category.GetALL();
            ViewBag.categories = categories;
            if (id == 0 || id == null) return NotFound();
            if (ModelState.IsValid)
            {
                List<ProductImage> UpdatedImages = new List<ProductImage>();

                foreach(var file in NewImages)
                {
                    var newimage= GetImageURL(file, model.Category_id);
                    var Image_Product = new ProductImage
                    {
                        ImageURL = newimage,
                    
                        Product_Id = model.Id
                    };
                    UpdatedImages.Add(Image_Product);
                }
                model.ProductImages = UpdatedImages;



                unit.Product.Update(model);
                unit.save();
                TempData["success"] = "Product updated syccesfully";
                return RedirectToAction("index");
            }
            return View(model);
        }

        private void DeleteImageMethod(ProductImage image)
        {
            if (image != null && !string.IsNullOrEmpty(image.ImageURL))
            {
                var relativePath = image.ImageURL.TrimStart('\\', '/'); 
                var oldImagePath = Path.Combine(env.WebRootPath, relativePath);

                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }
        }
           public IActionResult DeleteImage(int imageId)
            {
            var image = unit.ProductImage.GetByFilter(u => u.id == imageId);
            int productId = image.Product_Id;
            DeleteImageMethod(image);

                unit.ProductImage.Delete(image);
                unit.save();

                TempData["success"] = "Deleted successfully";
            

            return RedirectToAction("Edit", new { id = productId });
        }

        public string GetImageURL(IFormFile ImageFile,int product_id)
        {
            if (ImageFile == null || ImageFile.Length == 0)
            {
                return null;
            }

            string folderpath = Path.Combine(env.WebRootPath, "Images/Product"+product_id);
            if (!Directory.Exists(folderpath))
            {
                Directory.CreateDirectory(folderpath);
            }
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
            string path = Path.Combine(folderpath, fileName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                ImageFile.CopyTo(stream);
            }
            return "/Images/Product" + product_id + "/" + fileName;

        }


        //API Calls
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> products = unit.Product.GetALL(Includes: "Category");
            return Json(new { data = products });
        }

        [HttpDelete]
       public IActionResult Delete(int? id)
        {
            Product? model = unit.Product.GetByFilter(c => c.Id == id,Includes: "ProductImages");
            if (model == null) return Json(new { success = false, Message = "error while deleting" });
          foreach(var image in model.ProductImages)
            {
                DeleteImageMethod(image);
            }
            unit.Product.Delete(model);
              unit.save();

            return Json(new { success=true ,Message = "Delete successful" });
        }


    }
}
