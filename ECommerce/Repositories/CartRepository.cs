using System.Collections.Generic;
using System.Linq;
using Dapper;
using ECommerce.Data;
using ECommerce.Models;

namespace ECommerce.Repositories
{
    public class CartRepository : ICartRepository
    {
        public List<CartItem> GetBySessionId(string sessionId)
        {
            using (var connection = DatabaseConfig.GetConnection())
            {
                const string sql = @"
                    SELECT c.id as Id, c.product_id as ProductId, c.quantity as Quantity,
                           p.name as ProductName, p.image as ProductImage, p.price as Price,
                           p.type as Type, (p.price * c.quantity) as TotalPrice
                    FROM cart_items c
                    INNER JOIN products p ON c.product_id = p.id
                    WHERE c.session_id = @SessionId
                    ORDER BY c.created_at DESC";
                return connection.Query<CartItem>(sql, new { SessionId = sessionId }).ToList();
            }
        }

        public List<CartItem> GetByUserId(int userId)
        {
            using (var connection = DatabaseConfig.GetConnection())
            {
                const string sql = @"
                    SELECT c.id as Id, c.product_id as ProductId, c.quantity as Quantity,
                           p.name as ProductName, p.image as ProductImage, p.price as Price,
                           p.type as Type, (p.price * c.quantity) as TotalPrice
                    FROM cart_items c
                    INNER JOIN products p ON c.product_id = p.id
                    WHERE c.user_id = @UserId
                    ORDER BY c.created_at DESC";
                return connection.Query<CartItem>(sql, new { UserId = userId }).ToList();
            }
        }

        public CartItem AddItem(string sessionId, int? userId, int productId, int quantity)
        {
            using (var connection = DatabaseConfig.GetConnection())
            {
                // Check if item already exists in cart
                const string checkSql = @"
                    SELECT id FROM cart_items 
                    WHERE session_id = @SessionId AND product_id = @ProductId";
                var existingId = connection.ExecuteScalar<int?>(checkSql, new { SessionId = sessionId, ProductId = productId });

                if (existingId.HasValue)
                {
                    // Update quantity
                    const string updateSql = @"
                        UPDATE cart_items 
                        SET quantity = quantity + @Quantity 
                        WHERE id = @Id";
                    connection.Execute(updateSql, new { Id = existingId.Value, Quantity = quantity });
                }
                else
                {
                    // Insert new item
                    const string insertSql = @"
                        INSERT INTO cart_items (session_id, user_id, product_id, quantity) 
                        VALUES (@SessionId, @UserId, @ProductId, @Quantity)";
                    connection.Execute(insertSql, new { SessionId = sessionId, UserId = userId, ProductId = productId, Quantity = quantity });
                }

                // Return the cart item with product details
                const string selectSql = @"
                    SELECT c.id as Id, c.product_id as ProductId, c.quantity as Quantity,
                           p.name as ProductName, p.image as ProductImage, p.price as Price,
                           p.type as Type, (p.price * c.quantity) as TotalPrice
                    FROM cart_items c
                    INNER JOIN products p ON c.product_id = p.id
                    WHERE c.session_id = @SessionId AND c.product_id = @ProductId";
                return connection.QueryFirstOrDefault<CartItem>(selectSql, new { SessionId = sessionId, ProductId = productId });
            }
        }

        public bool UpdateQuantity(string sessionId, int productId, int quantity)
        {
            using (var connection = DatabaseConfig.GetConnection())
            {
                if (quantity <= 0)
                {
                    return RemoveItem(sessionId, productId);
                }

                const string sql = @"
                    UPDATE cart_items 
                    SET quantity = @Quantity 
                    WHERE session_id = @SessionId AND product_id = @ProductId";
                var rowsAffected = connection.Execute(sql, new { SessionId = sessionId, ProductId = productId, Quantity = quantity });
                return rowsAffected > 0;
            }
        }

        public bool RemoveItem(string sessionId, int productId)
        {
            using (var connection = DatabaseConfig.GetConnection())
            {
                const string sql = @"
                    DELETE FROM cart_items 
                    WHERE session_id = @SessionId AND product_id = @ProductId";
                var rowsAffected = connection.Execute(sql, new { SessionId = sessionId, ProductId = productId });
                return rowsAffected > 0;
            }
        }

        public bool ClearCart(string sessionId)
        {
            using (var connection = DatabaseConfig.GetConnection())
            {
                const string sql = "DELETE FROM cart_items WHERE session_id = @SessionId";
                connection.Execute(sql, new { SessionId = sessionId });
                return true;
            }
        }

        public bool TransferCartToUser(string sessionId, int userId)
        {
            using (var connection = DatabaseConfig.GetConnection())
            {
                const string sql = @"
                    UPDATE cart_items 
                    SET user_id = @UserId 
                    WHERE session_id = @SessionId";
                connection.Execute(sql, new { SessionId = sessionId, UserId = userId });
                return true;
            }
        }
    }
}
