using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ECommerce
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // API Routes - Products
            routes.MapRoute(
                name: "ApiProductsGetAll",
                url: "api/products",
                defaults: new { controller = "ProductsApi", action = "GetAll" }
            );

            routes.MapRoute(
                name: "ApiProductsGetById",
                url: "api/products/get/{id}",
                defaults: new { controller = "ProductsApi", action = "GetById" }
            );

            routes.MapRoute(
                name: "ApiProductsGetByType",
                url: "api/products/type/{type}",
                defaults: new { controller = "ProductsApi", action = "GetByType" }
            );

            routes.MapRoute(
                name: "ApiProductsSearch",
                url: "api/products/search",
                defaults: new { controller = "ProductsApi", action = "Search" }
            );

            routes.MapRoute(
                name: "ApiProductsFavorites",
                url: "api/products/favorites",
                defaults: new { controller = "ProductsApi", action = "GetFavorites" }
            );

            routes.MapRoute(
                name: "ApiProductsCreate",
                url: "api/products/create",
                defaults: new { controller = "ProductsApi", action = "Create" }
            );

            routes.MapRoute(
                name: "ApiProductsUpdate",
                url: "api/products/update",
                defaults: new { controller = "ProductsApi", action = "Update" }
            );

            routes.MapRoute(
                name: "ApiProductsDelete",
                url: "api/products/delete/{id}",
                defaults: new { controller = "ProductsApi", action = "Delete" }
            );

            // API Routes - Cart
            routes.MapRoute(
                name: "ApiCartGet",
                url: "api/cart",
                defaults: new { controller = "CartApi", action = "GetCart" }
            );

            routes.MapRoute(
                name: "ApiCartAdd",
                url: "api/cart/add",
                defaults: new { controller = "CartApi", action = "AddToCart" }
            );

            routes.MapRoute(
                name: "ApiCartUpdate",
                url: "api/cart/update",
                defaults: new { controller = "CartApi", action = "UpdateQuantity" }
            );

            routes.MapRoute(
                name: "ApiCartRemove",
                url: "api/cart/remove",
                defaults: new { controller = "CartApi", action = "RemoveFromCart" }
            );

            routes.MapRoute(
                name: "ApiCartClear",
                url: "api/cart/clear",
                defaults: new { controller = "CartApi", action = "ClearCart" }
            );

            routes.MapRoute(
                name: "ApiCartCount",
                url: "api/cart/count",
                defaults: new { controller = "CartApi", action = "GetCartCount" }
            );

            // API Routes - Orders
            routes.MapRoute(
                name: "ApiOrdersGetAll",
                url: "api/orders",
                defaults: new { controller = "OrdersApi", action = "GetAll" }
            );

            routes.MapRoute(
                name: "ApiOrdersGetById",
                url: "api/orders/get/{id}",
                defaults: new { controller = "OrdersApi", action = "GetById" }
            );

            routes.MapRoute(
                name: "ApiOrdersUser",
                url: "api/orders/user",
                defaults: new { controller = "OrdersApi", action = "GetUserOrders" }
            );

            routes.MapRoute(
                name: "ApiOrdersRecent",
                url: "api/orders/recent/{count}",
                defaults: new { controller = "OrdersApi", action = "GetRecentOrders", count = 10 }
            );

            routes.MapRoute(
                name: "ApiOrdersCheckout",
                url: "api/orders/checkout",
                defaults: new { controller = "OrdersApi", action = "Checkout" }
            );

            routes.MapRoute(
                name: "ApiOrdersStatus",
                url: "api/orders/status",
                defaults: new { controller = "OrdersApi", action = "UpdateStatus" }
            );

            routes.MapRoute(
                name: "ApiOrdersStats",
                url: "api/orders/stats",
                defaults: new { controller = "OrdersApi", action = "GetStats" }
            );

            // API Routes - Auth
            routes.MapRoute(
                name: "ApiAuthLogin",
                url: "api/auth/login",
                defaults: new { controller = "AuthApi", action = "Login" }
            );

            routes.MapRoute(
                name: "ApiAuthRegister",
                url: "api/auth/register",
                defaults: new { controller = "AuthApi", action = "Register" }
            );

            routes.MapRoute(
                name: "ApiAuthLogout",
                url: "api/auth/logout",
                defaults: new { controller = "AuthApi", action = "Logout" }
            );

            routes.MapRoute(
                name: "ApiAuthCurrent",
                url: "api/auth/current",
                defaults: new { controller = "AuthApi", action = "GetCurrentUser" }
            );

            routes.MapRoute(
                name: "ApiAuthCheck",
                url: "api/auth/check",
                defaults: new { controller = "AuthApi", action = "CheckAuth" }
            );

            // API Routes - Users
            routes.MapRoute(
                name: "ApiUsersGetAll",
                url: "api/users",
                defaults: new { controller = "UsersApi", action = "GetAll" }
            );

            routes.MapRoute(
                name: "ApiUsersGetById",
                url: "api/users/get/{id}",
                defaults: new { controller = "UsersApi", action = "GetById" }
            );

            routes.MapRoute(
                name: "ApiUsersCustomers",
                url: "api/users/customers",
                defaults: new { controller = "UsersApi", action = "GetCustomers" }
            );

            routes.MapRoute(
                name: "ApiUsersUpdate",
                url: "api/users/update",
                defaults: new { controller = "UsersApi", action = "Update" }
            );

            routes.MapRoute(
                name: "ApiUsersDelete",
                url: "api/users/delete/{id}",
                defaults: new { controller = "UsersApi", action = "Delete" }
            );

            routes.MapRoute(
                name: "ApiUsersToggleStatus",
                url: "api/users/toggle-status/{id}",
                defaults: new { controller = "UsersApi", action = "ToggleStatus" }
            );

            routes.MapRoute(
                name: "ApiUsersStats",
                url: "api/users/stats",
                defaults: new { controller = "UsersApi", action = "GetStats" }
            );

            // Default Route
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
