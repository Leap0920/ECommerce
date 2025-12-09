using System.Collections.Generic;
using System.Linq;
using Dapper;
using ECommerce.Data;
using ECommerce.Models;

namespace ECommerce.Repositories
{
    public class ProductRepository : IProductRepository
    {
        public List<Product> GetAll()
        {
            using (var connection = DatabaseConfig.GetConnection())
            {
                const string sql = @"
                    SELECT id as Id, name as Name, description as Description, 
                           price as Price, image as Image, type as Type, 
                           is_favorite as Favorite, stock_quantity as StockQuantity 
                    FROM products 
                    WHERE is_active = 1 
                    ORDER BY id DESC";
                return connection.Query<Product>(sql).ToList();
            }
        }

        public Product GetById(int id)
        {
            using (var connection = DatabaseConfig.GetConnection())
            {
                const string sql = @"
                    SELECT id as Id, name as Name, description as Description, 
                           price as Price, image as Image, type as Type, 
                           is_favorite as Favorite, stock_quantity as StockQuantity 
                    FROM products 
                    WHERE id = @Id";
                return connection.QueryFirstOrDefault<Product>(sql, new { Id = id });
            }
        }

        public List<Product> GetByType(string type)
        {
            using (var connection = DatabaseConfig.GetConnection())
            {
                if (string.IsNullOrEmpty(type) || type.ToLower() == "all")
                {
                    return GetAll();
                }

                const string sql = @"
                    SELECT id as Id, name as Name, description as Description, 
                           price as Price, image as Image, type as Type, 
                           is_favorite as Favorite, stock_quantity as StockQuantity 
                    FROM products 
                    WHERE is_active = 1 AND LOWER(type) = LOWER(@Type) 
                    ORDER BY id DESC";
                return connection.Query<Product>(sql, new { Type = type }).ToList();
            }
        }

        public List<Product> Search(string searchTerm)
        {
            using (var connection = DatabaseConfig.GetConnection())
            {
                if (string.IsNullOrEmpty(searchTerm))
                {
                    return GetAll();
                }

                const string sql = @"
                    SELECT id as Id, name as Name, description as Description, 
                           price as Price, image as Image, type as Type, 
                           is_favorite as Favorite, stock_quantity as StockQuantity 
                    FROM products 
                    WHERE is_active = 1 AND (LOWER(name) LIKE LOWER(@SearchTerm) OR LOWER(description) LIKE LOWER(@SearchTerm)) 
                    ORDER BY id DESC";
                return connection.Query<Product>(sql, new { SearchTerm = $"%{searchTerm}%" }).ToList();
            }
        }

        public List<Product> GetFavorites()
        {
            using (var connection = DatabaseConfig.GetConnection())
            {
                const string sql = @"
                    SELECT id as Id, name as Name, description as Description, 
                           price as Price, image as Image, type as Type, 
                           is_favorite as Favorite, stock_quantity as StockQuantity 
                    FROM products 
                    WHERE is_active = 1 AND is_favorite = 1 
                    ORDER BY id DESC";
                return connection.Query<Product>(sql).ToList();
            }
        }

        public Product Create(Product product)
        {
            using (var connection = DatabaseConfig.GetConnection())
            {
                const string sql = @"
                    INSERT INTO products (name, description, price, image, type, is_favorite, stock_quantity, is_active) 
                    VALUES (@Name, @Description, @Price, @Image, @Type, @Favorite, @StockQuantity, 1);
                    SELECT LAST_INSERT_ID();";
                
                product.Id = connection.ExecuteScalar<int>(sql, new
                {
                    product.Name,
                    product.Description,
                    product.Price,
                    product.Image,
                    product.Type,
                    product.Favorite,
                    product.StockQuantity
                });
                return product;
            }
        }

        public bool Update(Product product)
        {
            using (var connection = DatabaseConfig.GetConnection())
            {
                const string sql = @"
                    UPDATE products 
                    SET name = @Name, 
                        description = @Description, 
                        price = @Price, 
                        image = @Image, 
                        type = @Type, 
                        is_favorite = @Favorite,
                        stock_quantity = @StockQuantity 
                    WHERE id = @Id";
                
                var rowsAffected = connection.Execute(sql, product);
                return rowsAffected > 0;
            }
        }

        public bool Delete(int id)
        {
            using (var connection = DatabaseConfig.GetConnection())
            {
                // Soft delete - set is_active to 0
                const string sql = "UPDATE products SET is_active = 0 WHERE id = @Id";
                var rowsAffected = connection.Execute(sql, new { Id = id });
                return rowsAffected > 0;
            }
        }

        public bool UpdateStock(int productId, int quantity)
        {
            using (var connection = DatabaseConfig.GetConnection())
            {
                connection.Open();
                
                // First, check current stock
                const string checkSql = "SELECT stock_quantity FROM products WHERE id = @ProductId";
                var currentStock = connection.ExecuteScalar<int?>(checkSql, new { ProductId = productId });
                System.Diagnostics.Debug.WriteLine($"[UpdateStock] ProductId={productId}, CurrentStock={currentStock}, RequestedQty={quantity}");
                
                if (!currentStock.HasValue)
                {
                    System.Diagnostics.Debug.WriteLine($"[UpdateStock] ERROR: Product {productId} not found!");
                    return false;
                }
                
                if (currentStock.Value < quantity)
                {
                    System.Diagnostics.Debug.WriteLine($"[UpdateStock] WARNING: Insufficient stock for ProductId {productId}. Current: {currentStock.Value}, Requested: {quantity}. Updating anyway to prevent negative stock.");
                }
                
                // Update stock - allow going to 0 but not negative
                const string sql = @"
                    UPDATE products 
                    SET stock_quantity = GREATEST(0, stock_quantity - @Quantity)
                    WHERE id = @ProductId";
                var rowsAffected = connection.Execute(sql, new { ProductId = productId, Quantity = quantity });
                System.Diagnostics.Debug.WriteLine($"[UpdateStock] ProductId={productId}, Quantity={quantity}, RowsAffected={rowsAffected}");
                return rowsAffected > 0;
            }
        }
    }
}
