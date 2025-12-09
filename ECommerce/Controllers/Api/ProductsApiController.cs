using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using ECommerce.Models;
using ECommerce.Repositories;

namespace ECommerce.Controllers.Api
{
    public class ProductsApiController : Controller
    {
        private readonly IProductRepository _productRepository;

        public ProductsApiController()
        {
            _productRepository = new ProductRepository();
        }

        // GET: api/products
        [HttpGet]
        public ActionResult GetAll()
        {
            try
            {
                var products = _productRepository.GetAll();
                return Json(new { success = true, data = products }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: api/products/{id}
        [HttpGet]
        public ActionResult GetById(int id)
        {
            try
            {
                var product = _productRepository.GetById(id);
                if (product == null)
                {
                    return Json(new { success = false, message = "Product not found" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { success = true, data = product }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: api/products/type/{type}
        [HttpGet]
        public ActionResult GetByType(string type)
        {
            try
            {
                var products = _productRepository.GetByType(type);
                return Json(new { success = true, data = products }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: api/products/search?q={searchTerm}
        [HttpGet]
        public ActionResult Search(string q)
        {
            try
            {
                var products = _productRepository.Search(q);
                return Json(new { success = true, data = products }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: api/products/favorites
        [HttpGet]
        public ActionResult GetFavorites()
        {
            try
            {
                var products = _productRepository.GetFavorites();
                return Json(new { success = true, data = products }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // POST: api/products/create
        [HttpPost]
        public ActionResult Create()
        {
            try
            {
                var product = new Product
                {
                    Name = Request.Form["Name"],
                    Type = Request.Form["Type"],
                    Description = Request.Form["Description"]
                };

                if (decimal.TryParse(Request.Form["Price"], out decimal price))
                {
                    product.Price = price;
                }

                if (int.TryParse(Request.Form["StockQuantity"], out int stock))
                {
                    product.StockQuantity = stock;
                }

                if (string.IsNullOrEmpty(product.Name))
                {
                    return Json(new { success = false, message = "Product name is required" });
                }

                // Handle image upload
                if (Request.Files.Count > 0 && Request.Files["ImageFile"] != null && Request.Files["ImageFile"].ContentLength > 0)
                {
                    var file = Request.Files["ImageFile"];
                    product.Image = SaveUploadedImage(file);
                }
                else if (!string.IsNullOrEmpty(Request.Form["Image"]))
                {
                    product.Image = Request.Form["Image"];
                }

                var createdProduct = _productRepository.Create(product);
                return Json(new { success = true, data = createdProduct, message = "Product created successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: api/products/update
        [HttpPost]
        public ActionResult Update()
        {
            try
            {
                if (!int.TryParse(Request.Form["Id"], out int productId) || productId <= 0)
                {
                    return Json(new { success = false, message = "Invalid product ID" });
                }

                var product = new Product
                {
                    Id = productId,
                    Name = Request.Form["Name"],
                    Type = Request.Form["Type"],
                    Description = Request.Form["Description"]
                };

                if (decimal.TryParse(Request.Form["Price"], out decimal price))
                {
                    product.Price = price;
                }

                if (int.TryParse(Request.Form["StockQuantity"], out int stock))
                {
                    product.StockQuantity = stock;
                }

                if (string.IsNullOrEmpty(product.Name))
                {
                    return Json(new { success = false, message = "Product name is required" });
                }

                // Handle image upload
                if (Request.Files.Count > 0 && Request.Files["ImageFile"] != null && Request.Files["ImageFile"].ContentLength > 0)
                {
                    var file = Request.Files["ImageFile"];
                    product.Image = SaveUploadedImage(file);
                }
                else if (!string.IsNullOrEmpty(Request.Form["Image"]))
                {
                    product.Image = Request.Form["Image"];
                }

                var result = _productRepository.Update(product);
                if (result)
                {
                    return Json(new { success = true, message = "Product updated successfully" });
                }
                return Json(new { success = false, message = "Product not found or update failed" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // DELETE: api/products/{id}
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                var result = _productRepository.Delete(id);
                if (result)
                {
                    return Json(new { success = true, message = "Product deleted successfully" });
                }
                return Json(new { success = false, message = "Product not found" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        private string SaveUploadedImage(HttpPostedFileBase file)
        {
            // Validate file type
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var extension = Path.GetExtension(file.FileName).ToLower();
            
            if (Array.IndexOf(allowedExtensions, extension) < 0)
            {
                throw new Exception("Invalid file type. Only JPG, PNG, GIF, and WebP images are allowed.");
            }

            // Validate file size (5MB max)
            if (file.ContentLength > 5 * 1024 * 1024)
            {
                throw new Exception("File size must be less than 5MB.");
            }

            // Create directory if it doesn't exist
            var uploadsDir = Server.MapPath("~/Content/Images/Products");
            if (!Directory.Exists(uploadsDir))
            {
                Directory.CreateDirectory(uploadsDir);
            }

            // Generate unique filename
            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadsDir, fileName);

            // Save the file
            file.SaveAs(filePath);

            // Return the relative path for storage in database
            return $"/Content/Images/Products/{fileName}";
        }
    }
}
