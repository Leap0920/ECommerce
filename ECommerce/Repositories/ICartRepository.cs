using System.Collections.Generic;
using ECommerce.Models;

namespace ECommerce.Repositories
{
    public interface ICartRepository
    {
        List<CartItem> GetBySessionId(string sessionId);
        List<CartItem> GetByUserId(int userId);
        CartItem AddItem(string sessionId, int? userId, int productId, int quantity);
        bool UpdateQuantity(string sessionId, int productId, int quantity);
        bool RemoveItem(string sessionId, int productId);
        bool ClearCart(string sessionId);
        bool TransferCartToUser(string sessionId, int userId);
    }
}
