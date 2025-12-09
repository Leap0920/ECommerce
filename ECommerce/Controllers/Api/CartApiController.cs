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
                var sessionId = GetSessionId();
                var cartItems = _cartRepository.GetBySessionId(sessionId);
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

                var sessionId = GetSessionId();
                var userId = GetCurrentUserId();

                var cartItem = _cartRepository.AddItem(sessionId, userId, productId, quantity);

                // Get updated cart
                var cartItems = _cartRepository.GetBySessionId(sessionId);
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
                var sessionId = GetSessionId();
                _cartRepository.UpdateQuantity(sessionId, productId, quantity);

                // Get updated cart
                var cartItems = _cartRepository.GetBySessionId(sessionId);
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
                var sessionId = GetSessionId();
                _cartRepository.RemoveItem(sessionId, productId);

                // Get updated cart
                var cartItems = _cartRepository.GetBySessionId(sessionId);
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
                var sessionId = GetSessionId();
                _cartRepository.ClearCart(sessionId);

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
                var sessionId = GetSessionId();
                var cartItems = _cartRepository.GetBySessionId(sessionId);
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
