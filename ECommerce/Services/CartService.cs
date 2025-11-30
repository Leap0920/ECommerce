using System.Collections.Generic;
using System.Linq;
using System.Web;
using ECommerce.Models;

namespace ECommerce.Services
{
    public class CartService
    {
        private readonly HttpContextBase _httpContext;

        public CartService(HttpContextBase httpContext)
        {
            _httpContext = httpContext;
        }

        private string GetCartKey()
        {
            if (_httpContext.Session != null)
            {
                return $"Cart_{_httpContext.Session.SessionID}";
            }
            // Fallback if session is not available
            return "Cart_Default";
        }

        public List<CartItem> GetCart()
        {
            var cartKey = GetCartKey();
            var cart = _httpContext.Cache[cartKey] as List<CartItem>;
            return cart ?? new List<CartItem>();
        }

        public void AddToCart(Product product, int quantity)
        {
            var cart = GetCart();
            var existingItem = cart.FirstOrDefault(c => c.ProductId == product.Id);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
                existingItem.TotalPrice = existingItem.Quantity * existingItem.Price;
            }
            else
            {
                cart.Add(new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    ProductImage = product.Image,
                    Price = product.Price,
                    Quantity = quantity,
                    TotalPrice = product.Price * quantity,
                    Type = product.Type
                });
            }

            SaveCart(cart);
        }

        public void UpdateCartItem(int productId, int quantity)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(c => c.ProductId == productId);
            if (item != null)
            {
                if (quantity <= 0)
                {
                    cart.Remove(item);
                }
                else
                {
                    item.Quantity = quantity;
                    item.TotalPrice = item.Quantity * item.Price;
                }
                SaveCart(cart);
            }
        }

        public void RemoveFromCart(int productId)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(c => c.ProductId == productId);
            if (item != null)
            {
                cart.Remove(item);
                SaveCart(cart);
            }
        }

        public void ClearCart()
        {
            var cartKey = GetCartKey();
            _httpContext.Cache.Remove(cartKey);
        }

        private void SaveCart(List<CartItem> cart)
        {
            var cartKey = GetCartKey();
            _httpContext.Cache.Insert(cartKey, cart, null, System.Web.Caching.Cache.NoAbsoluteExpiration, System.Web.Caching.Cache.NoSlidingExpiration);
        }

        public decimal GetCartTotal()
        {
            var cart = GetCart();
            return cart.Sum(c => c.TotalPrice);
        }

        public int GetCartItemCount()
        {
            var cart = GetCart();
            return cart.Sum(c => c.Quantity);
        }
    }
}

