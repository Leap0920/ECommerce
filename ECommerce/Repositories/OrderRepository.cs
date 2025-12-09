using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using ECommerce.Data;
using ECommerce.Models;

namespace ECommerce.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        public List<Order> GetAll()
        {
            using (var connection = DatabaseConfig.GetConnection())
            {
                const string sql = @"
                    SELECT id as Id, user_id as UserId, customer_name as CustomerName, 
                           customer_email as CustomerEmail, shipping_address as ShippingAddress,
                           city as City, state as State, zip_code as ZipCode,
                           subtotal as Subtotal, tax as Tax, total as Total,
                           status as Status, order_date as OrderDate 
                    FROM orders 
                    ORDER BY order_date DESC";
                
                var orders = connection.Query<Order>(sql).ToList();

                // Load order items for each order
                foreach (var order in orders)
                {
                    order.Items = GetOrderItems(order.Id);
                }

                return orders;
            }
        }

        public Order GetById(string orderId)
        {
            using (var connection = DatabaseConfig.GetConnection())
            {
                const string sql = @"
                    SELECT id as Id, user_id as UserId, customer_name as CustomerName, 
                           customer_email as CustomerEmail, shipping_address as ShippingAddress,
                           city as City, state as State, zip_code as ZipCode,
                           subtotal as Subtotal, tax as Tax, total as Total,
                           status as Status, order_date as OrderDate 
                    FROM orders 
                    WHERE id = @OrderId";
                
                var order = connection.QueryFirstOrDefault<Order>(sql, new { OrderId = orderId });
                
                if (order != null)
                {
                    order.Items = GetOrderItems(orderId);
                }

                return order;
            }
        }

        public List<Order> GetByUserId(int userId)
        {
            using (var connection = DatabaseConfig.GetConnection())
            {
                const string sql = @"
                    SELECT id as Id, user_id as UserId, customer_name as CustomerName, 
                           customer_email as CustomerEmail, shipping_address as ShippingAddress,
                           city as City, state as State, zip_code as ZipCode,
                           subtotal as Subtotal, tax as Tax, total as Total,
                           status as Status, order_date as OrderDate 
                    FROM orders 
                    WHERE user_id = @UserId
                    ORDER BY order_date DESC";
                
                var orders = connection.Query<Order>(sql, new { UserId = userId }).ToList();

                foreach (var order in orders)
                {
                    order.Items = GetOrderItems(order.Id);
                }

                return orders;
            }
        }

        public Order Create(Order order)
        {
            using (var connection = DatabaseConfig.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Generate order ID
                        var orderCount = connection.ExecuteScalar<int>("SELECT COUNT(*) FROM orders", transaction: transaction);
                        order.Id = $"ORD-{DateTime.Now:yyyyMMdd}-{orderCount + 1:D6}";
                        order.OrderDate = DateTime.Now;

                        if (string.IsNullOrEmpty(order.Status))
                        {
                            order.Status = "Pending";
                        }

                        // Insert order
                        const string orderSql = @"
                            INSERT INTO orders (id, user_id, customer_name, customer_email, 
                                              shipping_address, city, state, zip_code,
                                              subtotal, tax, total, status, order_date) 
                            VALUES (@Id, @UserId, @CustomerName, @CustomerEmail, 
                                   @ShippingAddress, @City, @State, @ZipCode,
                                   @Subtotal, @Tax, @Total, @Status, @OrderDate)";

                        connection.Execute(orderSql, new
                        {
                            order.Id,
                            order.UserId,
                            order.CustomerName,
                            order.CustomerEmail,
                            order.ShippingAddress,
                            order.City,
                            order.State,
                            order.ZipCode,
                            order.Subtotal,
                            order.Tax,
                            order.Total,
                            order.Status,
                            order.OrderDate
                        }, transaction);

                        // Insert order items
                        const string itemSql = @"
                            INSERT INTO order_items (order_id, product_id, product_name, product_image,
                                                    price, quantity, total_price, type) 
                            VALUES (@OrderId, @ProductId, @ProductName, @ProductImage,
                                   @Price, @Quantity, @TotalPrice, @Type)";

                        foreach (var item in order.Items)
                        {
                            connection.Execute(itemSql, new
                            {
                                OrderId = order.Id,
                                item.ProductId,
                                item.ProductName,
                                item.ProductImage,
                                item.Price,
                                item.Quantity,
                                item.TotalPrice,
                                item.Type
                            }, transaction);
                        }

                        transaction.Commit();
                        return order;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public bool UpdateStatus(string orderId, string status)
        {
            using (var connection = DatabaseConfig.GetConnection())
            {
                const string sql = "UPDATE orders SET status = @Status WHERE id = @OrderId";
                var rowsAffected = connection.Execute(sql, new { OrderId = orderId, Status = status });
                return rowsAffected > 0;
            }
        }

        public bool Delete(string orderId)
        {
            using (var connection = DatabaseConfig.GetConnection())
            {
                // Order items will be deleted automatically due to CASCADE
                const string sql = "DELETE FROM orders WHERE id = @OrderId";
                var rowsAffected = connection.Execute(sql, new { OrderId = orderId });
                return rowsAffected > 0;
            }
        }

        public decimal GetTotalSales()
        {
            using (var connection = DatabaseConfig.GetConnection())
            {
                const string sql = "SELECT COALESCE(SUM(total), 0) FROM orders WHERE status != 'Cancelled'";
                return connection.ExecuteScalar<decimal>(sql);
            }
        }

        public int GetTotalOrders()
        {
            using (var connection = DatabaseConfig.GetConnection())
            {
                const string sql = "SELECT COUNT(*) FROM orders";
                return connection.ExecuteScalar<int>(sql);
            }
        }

        public List<Order> GetRecentOrders(int count)
        {
            using (var connection = DatabaseConfig.GetConnection())
            {
                const string sql = @"
                    SELECT id as Id, user_id as UserId, customer_name as CustomerName, 
                           customer_email as CustomerEmail, shipping_address as ShippingAddress,
                           city as City, state as State, zip_code as ZipCode,
                           subtotal as Subtotal, tax as Tax, total as Total,
                           status as Status, order_date as OrderDate 
                    FROM orders 
                    ORDER BY order_date DESC
                    LIMIT @Count";
                
                return connection.Query<Order>(sql, new { Count = count }).ToList();
            }
        }

        private List<OrderItem> GetOrderItems(string orderId)
        {
            using (var connection = DatabaseConfig.GetConnection())
            {
                const string sql = @"
                    SELECT id as Id, order_id as OrderId, product_id as ProductId,
                           product_name as ProductName, product_image as ProductImage,
                           price as Price, quantity as Quantity, total_price as TotalPrice, type as Type
                    FROM order_items 
                    WHERE order_id = @OrderId";
                return connection.Query<OrderItem>(sql, new { OrderId = orderId }).ToList();
            }
        }
    }
}
