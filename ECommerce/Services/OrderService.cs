using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ECommerce.Models;

namespace ECommerce.Services
{
    public class OrderService
    {
        private readonly HttpContextBase _httpContext;

        public OrderService(HttpContextBase httpContext)
        {
            _httpContext = httpContext;
        }

        public List<Order> GetAllOrders()
        {
            return CacheDataService.GetOrders(_httpContext);
        }

        public Order GetOrderById(string orderId)
        {
            var orders = GetAllOrders();
            return orders.FirstOrDefault(o => o.Id == orderId);
        }

        public List<Order> GetOrdersByUserId(int userId)
        {
            var orders = GetAllOrders();
            return orders.Where(o => o.UserId == userId).ToList();
        }

        public string CreateOrder(Order order)
        {
            var orders = GetAllOrders();
            order.Id = $"ORD-{DateTime.Now:yyyyMMdd}-{orders.Count + 1:D6}";
            order.OrderDate = DateTime.Now;
            if (string.IsNullOrEmpty(order.Status))
            {
                order.Status = "Pending";
            }
            orders.Insert(0, order); // Add to beginning
            _httpContext.Cache["Orders"] = orders;
            return order.Id;
        }

        public void UpdateOrderStatus(string orderId, string status)
        {
            var orders = GetAllOrders();
            var order = orders.FirstOrDefault(o => o.Id == orderId);
            if (order != null)
            {
                order.Status = status;
                _httpContext.Cache["Orders"] = orders;
            }
        }

        public decimal GetTotalSales()
        {
            var orders = GetAllOrders();
            return orders.Sum(o => o.Total);
        }

        public int GetTotalOrders()
        {
            return GetAllOrders().Count;
        }
    }
}

