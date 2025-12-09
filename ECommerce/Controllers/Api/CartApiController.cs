using System;
using System.Linq;
using System.Web.Mvc;
using ECommerce.Models;
using ECommerce.Repositories;

namespace ECommerce.Controllers.Api
{
    public class CartApiController : Controller
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;

        public CartApiController()
        {
            _cartRepository = new CartRepository();
            _productRepository = new ProductRepository();
        }

        private string GetSessionId()
        {
            if (Session != null)
            {
                return Session.SessionID;
            }
            return "Default";
        }

        private int? GetCurrentUserId()
        {
            if (Session["UserId"] != null)
            {
                return (int)Session["UserId"];
            }
            return null;
        }

        // GET: api/cart
        [HttpGet]
        public ActionResult GetCart()
        {
            try
            {
                var userId = GetCurrentUserId();
                var sessionId = GetSessionId();
                
                // If user is logged in, get cart by user_id; otherwise use session_id
                var cartItems = userId.HasValue 
                    ? _cartRepository.GetByUserId(userId.Value)
                    : _cartRepository.GetBySessionId(sessionId);
                    
                var total = cartItems.Sum(c => c.TotalPrice);
                var itemCount = cartItems.Sum(c => c.Quantity);

                return Json(new
                {
                    success = true,
                    data = new
                    {
                        items = cartItems,
                        total = total,
                        itemCount = itemCount
                    }
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // POST: api/cart/add
        [HttpPost]
        public ActionResult AddToCart(int productId, int quantity = 1)
        {
            try
            {
                var product = _productRepository.GetById(productId);
                if (product == null)
                {
                    return Json(new { success = false, message = "Product not found" });
                }

                var userId = GetCurrentUserId();
                var sessionId = GetSessionId();

                // Use user_id for logged-in users, session_id for guests
                CartItem cartItem;
                if (userId.HasValue)
                {
                    cartItem = _cartRepository.AddItemByUserId(userId.Value, productId, quantity);
                }
                else
                {
                    cartItem = _cartRepository.AddItem(sessionId, null, productId, quantity);
                }

                // Get updated cart
                var cartItems = userId.HasValue
                    ? _cartRepository.GetByUserId(userId.Value)
                    : _cartRepository.GetBySessionId(sessionId);
                var total = cartItems.Sum(c => c.TotalPrice);
                var itemCount = cartItems.Sum(c => c.Quantity);

                return Json(new
                {
                    success = true,
                    message = "Product added to cart",
                    data = new
                    {
                        item = cartItem,
                        total = total,
                        itemCount = itemCount
                    }
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: api/cart/update
        [HttpPost]
        public ActionResult UpdateQuantity(int productId, int quantity)
        {
            try
            {
                var userId = GetCurrentUserId();
                var sessionId = GetSessionId();

                // Use user_id for logged-in users
                if (userId.HasValue)
                {
                    _cartRepository.UpdateQuantityByUserId(userId.Value, productId, quantity);
                }
                else
                {
                    _cartRepository.UpdateQuantity(sessionId, productId, quantity);
                }

                // Get updated cart
                var cartItems = userId.HasValue
                    ? _cartRepository.GetByUserId(userId.Value)
                    : _cartRepository.GetBySessionId(sessionId);
                var total = cartItems.Sum(c => c.TotalPrice);
                var itemCount = cartItems.Sum(c => c.Quantity);

                return Json(new
                {
                    success = true,
                    message = "Cart updated",
                    data = new
                    {
                        items = cartItems,
                        total = total,
                        itemCount = itemCount
                    }
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: api/cart/remove
        [HttpPost]
        public ActionResult RemoveFromCart(int productId)
        {
            try
            {
                var userId = GetCurrentUserId();
                var sessionId = GetSessionId();

                // Use user_id for logged-in users
                if (userId.HasValue)
                {
                    _cartRepository.RemoveItemByUserId(userId.Value, productId);
                }
                else
                {
                    _cartRepository.RemoveItem(sessionId, productId);
                }

                // Get updated cart
                var cartItems = userId.HasValue
                    ? _cartRepository.GetByUserId(userId.Value)
                    : _cartRepository.GetBySessionId(sessionId);
                var total = cartItems.Sum(c => c.TotalPrice);
                var itemCount = cartItems.Sum(c => c.Quantity);

                return Json(new
                {
                    success = true,
                    message = "Item removed from cart",
                    data = new
                    {
                        items = cartItems,
                        total = total,
                        itemCount = itemCount
                    }
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: api/cart/clear
        [HttpPost]
        public ActionResult ClearCart()
        {
            try
            {
                var userId = GetCurrentUserId();
                var sessionId = GetSessionId();

                // Use user_id for logged-in users
                if (userId.HasValue)
                {
                    _cartRepository.ClearCartByUserId(userId.Value);
                }
                else
                {
                    _cartRepository.ClearCart(sessionId);
                }

                return Json(new
                {
                    success = true,
                    message = "Cart cleared",
                    data = new
                    {
                        items = new object[] { },
                        total = 0,
                        itemCount = 0
                    }
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // GET: api/cart/count
        [HttpGet]
        public ActionResult GetCartCount()
        {
            try
            {
                var userId = GetCurrentUserId();
                var sessionId = GetSessionId();

                var cartItems = userId.HasValue
                    ? _cartRepository.GetByUserId(userId.Value)
                    : _cartRepository.GetBySessionId(sessionId);
                var itemCount = cartItems.Sum(c => c.Quantity);

                return Json(new { success = true, count = itemCount }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}

