using System;
using System.Linq;
using System.Web.Mvc;
using ECommerce.Models;
using ECommerce.Repositories;

namespace ECommerce.Controllers.Api
{
    public class UsersApiController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IOrderRepository _orderRepository;

        public UsersApiController()
        {
            _userRepository = new UserRepository();
            _orderRepository = new OrderRepository();
        }

        // GET: api/users
        [HttpGet]
        public ActionResult GetAll()
        {
            try
            {
                var users = _userRepository.GetAll();
                // Don't return passwords
                var safeUsers = users.Select(u => new
                {
                    u.Id,
                    u.FirstName,
                    u.LastName,
                    u.Email,
                    u.Role,
                    u.IsActive,
                    u.CreatedDate
                }).ToList();

                return Json(new { success = true, data = safeUsers }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: api/users/{id}
        [HttpGet]
        public ActionResult GetById(int id)
        {
            try
            {
                var user = _userRepository.GetById(id);
                if (user == null)
                {
                    return Json(new { success = false, message = "User not found" }, JsonRequestBehavior.AllowGet);
                }

                return Json(new
                {
                    success = true,
                    data = new
                    {
                        user.Id,
                        user.FirstName,
                        user.LastName,
                        user.Email,
                        user.Role,
                        user.IsActive,
                        user.CreatedDate
                    }
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: api/users/customers
        [HttpGet]
        public ActionResult GetCustomers()
        {
            try
            {
                var users = _userRepository.GetAll()
                    .Where(u => u.Role == "Customer")
                    .Select(u => new
                    {
                        u.Id,
                        u.FirstName,
                        u.LastName,
                        u.Email,
                        u.Role,
                        u.IsActive,
                        u.CreatedDate
                    }).ToList();

                return Json(new { success = true, data = users }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // POST: api/users/update
        [HttpPost]
        public ActionResult Update(User user)
        {
            try
            {
                if (user == null || user.Id <= 0)
                {
                    return Json(new { success = false, message = "Invalid user data" });
                }

                // Get existing user to preserve password if not changed
                var existingUser = _userRepository.GetById(user.Id);
                if (existingUser == null)
                {
                    return Json(new { success = false, message = "User not found" });
                }

                // If password is empty, keep the old password
                if (string.IsNullOrEmpty(user.Password))
                {
                    user.Password = existingUser.Password;
                }

                var result = _userRepository.Update(user);
                if (result)
                {
                    return Json(new { success = true, message = "User updated successfully" });
                }
                return Json(new { success = false, message = "Failed to update user" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: api/users/delete
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                var result = _userRepository.Delete(id);
                if (result)
                {
                    return Json(new { success = true, message = "User deleted successfully" });
                }
                return Json(new { success = false, message = "User not found" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: api/users/toggle-status
        [HttpPost]
        public ActionResult ToggleStatus(int id)
        {
            try
            {
                var user = _userRepository.GetById(id);
                if (user == null)
                {
                    return Json(new { success = false, message = "User not found" });
                }

                user.IsActive = !user.IsActive;
                var result = _userRepository.Update(user);

                if (result)
                {
                    return Json(new
                    {
                        success = true,
                        message = user.IsActive ? "User activated" : "User deactivated",
                        isActive = user.IsActive
                    });
                }
                return Json(new { success = false, message = "Failed to update status" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // GET: api/users/stats
        [HttpGet]
        public ActionResult GetStats()
        {
            try
            {
                var users = _userRepository.GetAll();
                var totalCustomers = users.Count(u => u.Role == "Customer");
                var activeCustomers = users.Count(u => u.Role == "Customer" && u.IsActive);

                return Json(new
                {
                    success = true,
                    data = new
                    {
                        totalCustomers = totalCustomers,
                        activeCustomers = activeCustomers
                    }
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
