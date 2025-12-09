using System.Collections.Generic;
using ECommerce.Models;

namespace ECommerce.Repositories
{
    public interface IProductRepository
    {
        List<Product> GetAll();
        Product GetById(int id);
        List<Product> GetByType(string type);
        List<Product> Search(string searchTerm);
        List<Product> GetFavorites();
        Product Create(Product product);
        bool Update(Product product);
        bool Delete(int id);
        bool UpdateStock(int productId, int quantity);
    }
}
