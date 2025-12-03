using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Caching;
using ECommerce.Models;
using Newtonsoft.Json;

namespace ECommerce.Services
{
    public static class CacheDataService
    {
        private static readonly object _lockObject = new object();

        public static void InitializeCache(HttpContextBase httpContext)
        {
            lock (_lockObject)
            {
                // Initialize Products from JSON
                if (httpContext.Cache["Products"] == null)
                {
                    var products = LoadProductsFromJson(httpContext);
                    httpContext.Cache.Insert("Products", products, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration);
                }

                // Initialize Orders
                if (httpContext.Cache["Orders"] == null)
                {
                    httpContext.Cache.Insert("Orders", new List<Order>(), null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration);
                }

                // Initialize Users with default admin
                if (httpContext.Cache["Users"] == null)
                {
                    var users = new List<User>
                    {
                        new User
                        {
                            Id = 1,
                            FirstName = "Admin",
                            LastName = "User",
                            Email = "admin@ecommerce.com",
                            Password = "Admin123!", // In production, this should be hashed
                            Role = "Admin",
                            CreatedDate = DateTime.Now,
                            IsActive = true
                        }
                    };
                    httpContext.Cache.Insert("Users", users, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration);
                }
            }
        }

        private static List<Product> LoadProductsFromJson(HttpContextBase httpContext)
        {
            try
            {
                var jsonPath = httpContext.Server.MapPath("~/Content/Data/vegs.json");
                if (File.Exists(jsonPath))
                {
                    var jsonContent = File.ReadAllText(jsonPath);
                    var products = JsonConvert.DeserializeObject<List<Product>>(jsonContent);
                    return products ?? new List<Product>();
                }
            }
            catch (Exception ex)
            {
                // Log error in production
                System.Diagnostics.Debug.WriteLine($"Error loading products: {ex.Message}");
            }

            return new List<Product>();
        }

        public static List<Product> GetProducts(HttpContextBase httpContext)
        {
            InitializeCache(httpContext);
            return httpContext.Cache["Products"] as List<Product> ?? new List<Product>();
        }

        public static List<Order> GetOrders(HttpContextBase httpContext)
        {
            InitializeCache(httpContext);
            return httpContext.Cache["Orders"] as List<Order> ?? new List<Order>();
        }

        public static List<User> GetUsers(HttpContextBase httpContext)
        {
            InitializeCache(httpContext);
            return httpContext.Cache["Users"] as List<User> ?? new List<User>();
        }
    }
}

