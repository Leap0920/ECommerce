using System.Collections.Generic;
using ECommerce.Models;

namespace ECommerce.Repositories
{
    public interface ICartRepository
    {
        List<CartItem> GetBySessionId(string sessionId);
        List<CartItem> GetByUserId(int userId);
        CartItem AddItem(string sessionId, int? userId, int productId, int quantity);
        CartItem AddItemByUserId(int userId, int productId, int quantity);
        bool UpdateQuantity(string sessionId, int productId, int quantity);
        bool UpdateQuantityByUserId(int userId, int productId, int quantity);
        bool RemoveItem(string sessionId, int productId);
        bool RemoveItemByUserId(int userId, int productId);
        bool RemoveItemsByProductIds(int userId, List<int> productIds);
        bool ClearCart(string sessionId);
        bool ClearCartByUserId(int userId);
        bool TransferCartToUser(string sessionId, int userId);
    }
}
