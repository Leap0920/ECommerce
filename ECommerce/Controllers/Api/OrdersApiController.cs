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
        private readonly IProductRepository _productRepository;

        public OrdersApiController()
        {
            _orderRepository = new OrderRepository();
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

        // GET: api/orders
        [HttpGet]
        public ActionResult GetAll()
        {
            try
            {
                var orders = _orderRepository.GetAll();
                
                // Map to anonymous objects to ensure proper JSON serialization
                var result = orders.Select(o => new
                {
                    o.Id,
                    o.UserId,
                    o.CustomerName,
                    o.CustomerEmail,
                    o.Phone,
                    o.ShippingAddress,
                    o.City,
                    o.State,
                    o.ZipCode,
                    o.Subtotal,
                    o.Tax,
                    o.Total,
                    o.Status,
                    OrderDate = o.OrderDate.ToString("yyyy-MM-ddTHH:mm:ss"),
                    Items = o.Items?.Select(i => new
                    {
                        i.Id,
                        OrderId = i.OrderId ?? o.Id,
                        i.ProductId,
                        i.ProductName,
                        i.ProductImage,
                        i.Price,
                        i.Quantity,
                        i.TotalPrice,
                        i.Type
                    }).ToList()
                }).ToList();
                
                return Json(new { success = true, data = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetAll Error: {ex.Message} | {ex.StackTrace}");
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
                
                // Map to anonymous objects for proper JSON serialization
                var result = orders.Select(o => new
                {
                    o.Id,
                    o.UserId,
                    o.CustomerName,
                    o.CustomerEmail,
                    o.Phone,
                    o.ShippingAddress,
                    o.City,
                    o.State,
                    o.ZipCode,
                    o.Subtotal,
                    o.Tax,
                    o.Total,
                    o.Status,
                    OrderDate = o.OrderDate.ToString("yyyy-MM-ddTHH:mm:ss"),
                    Items = o.Items?.Select(i => new
                    {
                        i.Id,
                        OrderId = i.OrderId ?? o.Id,
                        i.ProductId,
                        i.ProductName,
                        i.ProductImage,
                        i.Price,
                        i.Quantity,
                        i.TotalPrice,
                        i.Type
                    }).ToList()
                }).ToList();
                
                return Json(new { success = true, data = result }, JsonRequestBehavior.AllowGet);
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
                
                // Map to anonymous objects for proper JSON serialization
                var result = orders.Select(o => new
                {
                    o.Id,
                    o.UserId,
                    o.CustomerName,
                    o.CustomerEmail,
                    o.Phone,
                    o.ShippingAddress,
                    o.City,
                    o.State,
                    o.ZipCode,
                    o.Subtotal,
                    o.Tax,
                    o.Total,
                    o.Status,
                    OrderDate = o.OrderDate.ToString("yyyy-MM-ddTHH:mm:ss"),
                    Items = o.Items?.Select(i => new
                    {
                        i.Id,
                        OrderId = i.OrderId ?? o.Id,
                        i.ProductId,
                        i.ProductName,
                        i.ProductImage,
                        i.Price,
                        i.Quantity,
                        i.TotalPrice,
                        i.Type
                    }).ToList()
                }).ToList();
                
                return Json(new { success = true, data = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // POST: api/orders/checkout
        [HttpPost]
        public ActionResult Checkout()
        {
            try
            {
                // Manually deserialize JSON from request body (ASP.NET MVC doesn't auto-bind JSON like Web API)
                CheckoutRequest request = null;
                try
                {
                    Request.InputStream.Position = 0;
                    using (var reader = new System.IO.StreamReader(Request.InputStream))
                    {
                        var json = reader.ReadToEnd();
                        if (!string.IsNullOrEmpty(json))
                        {
                            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                            request = serializer.Deserialize<CheckoutRequest>(json);
                        }
                    }
                }
                catch (Exception parseEx)
                {
                    return Json(new { success = false, message = "Failed to parse request: " + parseEx.Message });
                }

                if (request == null)
                {
                    return Json(new { success = false, message = "Invalid request data" });
                }

                var userId = GetCurrentUserId();
                var sessionId = GetSessionId();
                
                // Get cart items - use user_id for logged-in users, session_id for guests
                System.Diagnostics.Debug.WriteLine($"[Checkout] Getting cart items for UserId: {userId}, SessionId: {sessionId}");
                var cartItems = userId.HasValue
                    ? _cartRepository.GetByUserId(userId.Value)
                    : _cartRepository.GetBySessionId(sessionId);

                System.Diagnostics.Debug.WriteLine($"[Checkout] Found {cartItems.Count()} cart items");
                foreach (var ci in cartItems)
                {
                    System.Diagnostics.Debug.WriteLine($"[Checkout] Cart Item - ProductId: {ci.ProductId}, Name: {ci.ProductName}, Qty: {ci.Quantity}");
                }

                if (!cartItems.Any())
                {
                    return Json(new { success = false, message = "Cart is empty" });
                }

                var subtotal = cartItems.Sum(c => c.TotalPrice);
                var tax = subtotal * 0.12m; // 12% VAT
                var total = subtotal + tax;

                var order = new Order
                {
                    UserId = userId,
                    CustomerName = request.CustomerName,
                    CustomerEmail = request.CustomerEmail,
                    Phone = request.Phone,
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
                System.Diagnostics.Debug.WriteLine($"[Checkout] Order created: {createdOrder.Id}");

                // Update stock for each product in the order
                var itemsList = cartItems.ToList();
                System.Diagnostics.Debug.WriteLine($"[Checkout] Updating stock for {itemsList.Count} items");
                foreach (var item in itemsList)
                {
                    System.Diagnostics.Debug.WriteLine($"[Checkout] Updating stock - ProductId: {item.ProductId}, Quantity: {item.Quantity}, ProductName: {item.ProductName}");
                    try
                    {
                        var stockUpdated = _productRepository.UpdateStock(item.ProductId, item.Quantity);
                        System.Diagnostics.Debug.WriteLine($"[Checkout] Stock update result for ProductId {item.ProductId}: {stockUpdated}");
                        if (!stockUpdated)
                        {
                            System.Diagnostics.Debug.WriteLine($"[Checkout] WARNING: Stock update returned false for ProductId {item.ProductId}. Product may not exist or insufficient stock.");
                        }
                    }
                    catch (Exception stockEx)
                    {
                        System.Diagnostics.Debug.WriteLine($"[Checkout] ERROR updating stock for ProductId {item.ProductId}: {stockEx.Message}");
                    }
                }

                // Clear the cart after successful order - use user_id for logged-in users
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
        public string Phone { get; set; }
        public string ShippingAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
    }
}
