using System;
using System.Web.Mvc;
using ECommerce.Models;
using ECommerce.Models.ViewModels;
using ECommerce.Repositories;
using BCrypt.Net;

namespace ECommerce.Controllers.Api
{
    public class AuthApiController : Controller
    {
        private readonly IUserRepository _userRepository;

        public AuthApiController()
        {
            _userRepository = new UserRepository();
        }

        // POST: api/auth/login
        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            try
            {
                if (model == null || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
                {
                    return Json(new { success = false, message = "Email and password are required" });
                }

                var user = _userRepository.GetByEmail(model.Email);
                if (user == null)
                {
                    return Json(new { success = false, message = "Invalid email or password" });
                }

                // For production, use BCrypt to verify password
                // bool isValidPassword = BCrypt.Net.BCrypt.Verify(model.Password, user.Password);
                // For simplicity, comparing plain text (NOT RECOMMENDED for production)
                if (user.Password != model.Password)
                {
                    return Json(new { success = false, message = "Invalid email or password" });
                }

                if (!user.IsActive)
                {
                    return Json(new { success = false, message = "Account is inactive" });
                }

                // Store user info in session
                Session["UserId"] = user.Id;
                Session["UserEmail"] = user.Email;
                Session["UserName"] = $"{user.FirstName} {user.LastName}";
                Session["UserRole"] = user.Role;

                return Json(new
                {
                    success = true,
                    message = "Login successful",
                    data = new
                    {
                        id = user.Id,
                        firstName = user.FirstName,
                        lastName = user.LastName,
                        email = user.Email,
                        role = user.Role
                    }
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: api/auth/register
        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            try
            {
                if (model == null)
                {
                    return Json(new { success = false, message = "Invalid registration data" });
                }

                if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
                {
                    return Json(new { success = false, message = "Email and password are required" });
                }

                if (model.Password != model.ConfirmPassword)
                {
                    return Json(new { success = false, message = "Passwords do not match" });
                }

                // Check if email already exists
                if (_userRepository.EmailExists(model.Email))
                {
                    return Json(new { success = false, message = "Email already registered" });
                }

                // Create new user
                var user = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    // For production, hash the password: BCrypt.Net.BCrypt.HashPassword(model.Password)
                    Password = model.Password,
                    Role = "Customer",
                    IsActive = true,
                    CreatedDate = DateTime.Now
                };

                var createdUser = _userRepository.Create(user);

                // Store user info in session (auto-login after registration)
                Session["UserId"] = createdUser.Id;
                Session["UserEmail"] = createdUser.Email;
                Session["UserName"] = $"{createdUser.FirstName} {createdUser.LastName}";
                Session["UserRole"] = createdUser.Role;

                return Json(new
                {
                    success = true,
                    message = "Registration successful",
                    data = new
                    {
                        id = createdUser.Id,
                        firstName = createdUser.FirstName,
                        lastName = createdUser.LastName,
                        email = createdUser.Email,
                        role = createdUser.Role
                    }
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: api/auth/logout
        [HttpPost]
        public ActionResult Logout()
        {
            try
            {
                Session.Clear();
                Session.Abandon();

                return Json(new { success = true, message = "Logged out successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // GET: api/auth/current
        [HttpGet]
        public ActionResult GetCurrentUser()
        {
            try
            {
                if (Session["UserId"] == null)
                {
                    return Json(new { success = false, message = "Not logged in" }, JsonRequestBehavior.AllowGet);
                }

                return Json(new
                {
                    success = true,
                    data = new
                    {
                        id = Session["UserId"],
                        email = Session["UserEmail"],
                        name = Session["UserName"],
                        role = Session["UserRole"]
                    }
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: api/auth/check
        [HttpGet]
        public ActionResult CheckAuth()
        {
            try
            {
                var isLoggedIn = Session["UserId"] != null;
                return Json(new
                {
                    success = true,
                    isLoggedIn = isLoggedIn,
                    role = Session["UserRole"]?.ToString()
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
