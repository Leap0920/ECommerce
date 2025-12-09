using System.Collections.Generic;
using ECommerce.Models;

namespace ECommerce.Repositories
{
    public interface IOrderRepository
    {
        List<Order> GetAll();
        Order GetById(string orderId);
        List<Order> GetByUserId(int userId);
        Order Create(Order order);
        bool UpdateStatus(string orderId, string status);
        bool Delete(string orderId);
        decimal GetTotalSales();
        int GetTotalOrders();
        List<Order> GetRecentOrders(int count);
    }
}
