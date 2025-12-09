using System;
using System.Linq;
using System.Web.Mvc;
using ECommerce.Models;
using ECommerce.Repositories;

namespace ECommerce.Controllers.Api
{
    public class OrdersApiController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;

        public OrdersApiController()
        {
            _orderRepository = new OrderRepository();
            _cartRepository = new CartRepository();
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

        // GET: api/orders
        [HttpGet]
        public ActionResult GetAll()
        {
            try
            {
                var orders = _orderRepository.GetAll();
                return Json(new { success = true, data = orders }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: api/orders/{id}
        [HttpGet]
        public ActionResult GetById(string id)
        {
            try
            {
                var order = _orderRepository.GetById(id);
                if (order == null)
                {
                    return Json(new { success = false, message = "Order not found" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { success = true, data = order }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: api/orders/user
        [HttpGet]
        public ActionResult GetUserOrders()
        {
            try
            {
                var userId = GetCurrentUserId();
                if (!userId.HasValue)
                {
                    return Json(new { success = false, message = "User not logged in" }, JsonRequestBehavior.AllowGet);
                }

                var orders = _orderRepository.GetByUserId(userId.Value);
                return Json(new { success = true, data = orders }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: api/orders/recent/{count}
        [HttpGet]
        public ActionResult GetRecentOrders(int count = 10)
        {
            try
            {
                var orders = _orderRepository.GetRecentOrders(count);
                return Json(new { success = true, data = orders }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // POST: api/orders/checkout
        [HttpPost]
        public ActionResult Checkout(CheckoutRequest request)
        {
            try
            {
                if (request == null)
                {
                    return Json(new { success = false, message = "Invalid request data" });
                }

                var sessionId = GetSessionId();
                var cartItems = _cartRepository.GetBySessionId(sessionId);

                if (!cartItems.Any())
                {
                    return Json(new { success = false, message = "Cart is empty" });
                }

                var subtotal = cartItems.Sum(c => c.TotalPrice);
                var tax = subtotal * 0.08m; // 8% tax
                var total = subtotal + tax;

                var order = new Order
                {
                    UserId = GetCurrentUserId(),
                    CustomerName = request.CustomerName,
                    CustomerEmail = request.CustomerEmail,
                    ShippingAddress = request.ShippingAddress,
                    City = request.City,
                    State = request.State,
                    ZipCode = request.ZipCode,
                    Subtotal = subtotal,
                    Tax = tax,
                    Total = total,
                    Status = "Pending",
                    Items = cartItems.Select(c => new OrderItem
                    {
                        ProductId = c.ProductId,
                        ProductName = c.ProductName,
                        ProductImage = c.ProductImage,
                        Price = c.Price,
                        Quantity = c.Quantity,
                        TotalPrice = c.TotalPrice,
                        Type = c.Type
                    }).ToList()
                };

                var createdOrder = _orderRepository.Create(order);

                // Clear the cart after successful order
                _cartRepository.ClearCart(sessionId);

                return Json(new
                {
                    success = true,
                    message = "Order placed successfully",
                    data = new
                    {
                        orderId = createdOrder.Id,
                        total = createdOrder.Total
                    }
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: api/orders/status
        [HttpPost]
        public ActionResult UpdateStatus(string orderId, string status)
        {
            try
            {
                if (string.IsNullOrEmpty(orderId) || string.IsNullOrEmpty(status))
                {
                    return Json(new { success = false, message = "Invalid request data" });
                }

                var result = _orderRepository.UpdateStatus(orderId, status);
                if (result)
                {
                    return Json(new { success = true, message = "Order status updated" });
                }
                return Json(new { success = false, message = "Order not found" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // GET: api/orders/stats
        [HttpGet]
        public ActionResult GetStats()
        {
            try
            {
                var totalSales = _orderRepository.GetTotalSales();
                var totalOrders = _orderRepository.GetTotalOrders();

                return Json(new
                {
                    success = true,
                    data = new
                    {
                        totalSales = totalSales,
                        totalOrders = totalOrders
                    }
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }

    public class CheckoutRequest
    {
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string ShippingAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
    }
}
