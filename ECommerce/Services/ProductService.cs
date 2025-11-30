using System.Collections.Generic;
using System.Linq;
using System.Web;
using ECommerce.Models;

namespace ECommerce.Services
{
    public class ProductService
    {
        private readonly HttpContextBase _httpContext;

        public ProductService(HttpContextBase httpContext)
        {
            _httpContext = httpContext;
        }

        public List<Product> GetAllProducts()
        {
            return CacheDataService.GetProducts(_httpContext);
        }

        public Product GetProductById(int id)
        {
            var products = GetAllProducts();
            return products.FirstOrDefault(p => p.Id == id);
        }

        public List<Product> GetProductsByType(string type)
        {
            var products = GetAllProducts();
            if (string.IsNullOrEmpty(type) || type.ToLower() == "all")
                return products;
            return products.Where(p => p.Type.ToLower() == type.ToLower()).ToList();
        }

        public List<Product> SearchProducts(string searchTerm)
        {
            var products = GetAllProducts();
            if (string.IsNullOrEmpty(searchTerm))
                return products;
            return products.Where(p => p.Name.ToLower().Contains(searchTerm.ToLower())).ToList();
        }

        public void AddProduct(Product product)
        {
            var products = GetAllProducts();
            if (products.Any())
            {
                product.Id = products.Max(p => p.Id) + 1;
            }
            else
            {
                product.Id = 1;
            }
            products.Add(product);
            _httpContext.Cache["Products"] = products;
        }

        public void UpdateProduct(Product product)
        {
            var products = GetAllProducts();
            var existingProduct = products.FirstOrDefault(p => p.Id == product.Id);
            if (existingProduct != null)
            {
                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price;
                existingProduct.Image = product.Image;
                existingProduct.Description = product.Description;
                existingProduct.Type = product.Type;
                existingProduct.Favorite = product.Favorite;
                _httpContext.Cache["Products"] = products;
            }
        }

        public void DeleteProduct(int id)
        {
            var products = GetAllProducts();
            var product = products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                products.Remove(product);
                _httpContext.Cache["Products"] = products;
            }
        }
    }
}

