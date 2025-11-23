---
description: Complete implementation plan for the e-commerce web application
---

# E-Commerce Application - Implementation Plan

## Project Overview
Transform the existing ASP.NET MVC project into a full-featured e-commerce application with MySQL database integration, user authentication, role-based authorization, and comprehensive CRUD operations.

---

## Phase 1: Database Foundation (Days 1-3)

### Step 1.1: Install MySQL and Create Database
**Duration**: 30 minutes

```sql
-- Create database
CREATE DATABASE ecommerce_db;
USE ecommerce_db;

-- Set character set (important for special characters)
ALTER DATABASE ecommerce_db CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
```

**Deliverable**: Working MySQL database instance

---

### Step 1.2: Create Database Schema
**Duration**: 2 hours

```sql
-- Users table with roles
CREATE TABLE Users (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Username VARCHAR(50) UNIQUE NOT NULL,
    Email VARCHAR(100) UNIQUE NOT NULL,
    PasswordHash VARCHAR(255) NOT NULL,
    Role VARCHAR(20) NOT NULL DEFAULT 'Customer', -- 'Admin', 'Customer', 'Guest'
    FirstName VARCHAR(50),
    LastName VARCHAR(50),
    PhoneNumber VARCHAR(20),
    IsActive BOOLEAN DEFAULT TRUE,
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    INDEX idx_email (Email),
    INDEX idx_role (Role)
);

-- Categories table
CREATE TABLE Categories (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Name VARCHAR(50) UNIQUE NOT NULL,
    Description TEXT,
    ImageUrl VARCHAR(255),
    Type VARCHAR(20), -- 'leafy', 'root', 'fruit'
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- Products table
CREATE TABLE Products (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Name VARCHAR(100) NOT NULL,
    Description TEXT,
    Price DECIMAL(10, 2) NOT NULL,
    CategoryId INT,
    ImageUrl VARCHAR(255),
    Stock INT DEFAULT 0,
    IsActive BOOLEAN DEFAULT TRUE,
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (CategoryId) REFERENCES Categories(Id) ON DELETE SET NULL,
    INDEX idx_category (CategoryId),
    INDEX idx_active (IsActive)
);

-- Orders table
CREATE TABLE Orders (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    UserId INT NOT NULL,
    OrderDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    TotalAmount DECIMAL(10, 2) NOT NULL,
    Status VARCHAR(20) DEFAULT 'Pending', -- 'Pending', 'Processing', 'Shipped', 'Delivered', 'Cancelled'
    ShippingAddress TEXT,
    PaymentMethod VARCHAR(50) DEFAULT 'Simulated',
    Notes TEXT,
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE,
    INDEX idx_user (UserId),
    INDEX idx_status (Status),
    INDEX idx_date (OrderDate)
);

-- OrderItems table
CREATE TABLE OrderItems (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    OrderId INT NOT NULL,
    ProductId INT NOT NULL,
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(10, 2) NOT NULL,
    Subtotal DECIMAL(10, 2) NOT NULL,
    FOREIGN KEY (OrderId) REFERENCES Orders(Id) ON DELETE CASCADE,
    FOREIGN KEY (ProductId) REFERENCES Products(Id) ON DELETE RESTRICT,
    INDEX idx_order (OrderId),
    INDEX idx_product (ProductId)
);

-- Cart table (optional - can use session instead)
CREATE TABLE Carts (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    UserId INT UNIQUE,
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
);

-- CartItems table
CREATE TABLE CartItems (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    CartId INT NOT NULL,
    ProductId INT NOT NULL,
    Quantity INT NOT NULL DEFAULT 1,
    AddedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (CartId) REFERENCES Carts(Id) ON DELETE CASCADE,
    FOREIGN KEY (ProductId) REFERENCES Products(Id) ON DELETE CASCADE,
    UNIQUE KEY unique_cart_product (CartId, ProductId),
    INDEX idx_cart (CartId)
);

-- Reviews table (BONUS FEATURE)
CREATE TABLE Reviews (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    ProductId INT NOT NULL,
    UserId INT NOT NULL,
    Rating INT NOT NULL CHECK (Rating BETWEEN 1 AND 5),
    Comment TEXT,
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (ProductId) REFERENCES Products(Id) ON DELETE CASCADE,
    FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE,
    INDEX idx_product (ProductId),
    INDEX idx_user (UserId)
);
```

**Deliverable**: Complete database schema with all tables

---

### Step 1.3: Seed Initial Data
**Duration**: 1 hour

```sql
-- Insert admin user (password: Admin123! - hashed)
INSERT INTO Users (Username, Email, PasswordHash, Role, FirstName, LastName) VALUES
('admin', 'admin@ecommerce.com', '$2a$11$ExampleHashHereWillBeReplacedByCode', 'Admin', 'Admin', 'User');

-- Insert categories
INSERT INTO Categories (Name, Description, Type) VALUES
('Leafy Vegetables', 'Fresh leafy greens', 'leafy'),
('Root Vegetables', 'Underground vegetables', 'root'),
('Fruit Vegetables', 'Vegetables that are botanically fruits', 'fruit');

-- Migrate existing products from JSON to database
INSERT INTO Products (Name, Description, Price, CategoryId, ImageUrl, Stock) VALUES
('Spinach', 'Fresh organic spinach leaves, rich in iron and vitamins', 2.50, 1, '/Content/spinach.jpg', 100),
('Broccoli', 'Green broccoli florets, high in fiber and vitamin C', 3.00, 1, '/Content/brocolli.jpg', 80),
('Cabbage', 'Fresh green cabbage, perfect for salads and cooking', 1.50, 1, '/Content/Cabbage.jpg', 120),
('Carrot', 'Crunchy orange carrots, excellent source of beta-carotene', 2.00, 2, '/Content/carrots.jpg', 150),
('Potato', 'Versatile potatoes for various cooking methods', 1.75, 2, '/Content/potato.jpg', 200),
('Onion', 'Fresh onions, essential for flavoring dishes', 1.25, 2, '/Content/onion.jpg', 180),
('Garlic', 'Aromatic garlic for seasoning', 3.50, 2, '/Content/Garlic.jpg', 90),
('Tomato', 'Ripe red tomatoes, great for salads and sauces', 2.75, 3, '/Content/tomato-new.jpg', 110),
('Bell Pepper', 'Colorful bell peppers, sweet and nutritious', 3.25, 3, '/Content/bell-pepper.jpg', 70),
('Eggplant', 'Purple eggplant for grilling and roasting', 2.50, 3, '/Content/eggplant.jpeg', 60);
```

**Deliverable**: Database with admin user, categories, and products

---

### Step 1.4: Install NuGet Packages
**Duration**: 30 minutes

```powershell
# Navigate to project directory
cd c:\Users\Justin\source\repos\MVC-Ecommerce-IPT\ECommerce

# Install Entity Framework 6
Install-Package EntityFramework -Version 6.4.4

# Install MySQL Data Provider
Install-Package MySql.Data -Version 8.0.33
Install-Package MySql.Data.EntityFramework -Version 8.0.33

# Install BCrypt for password hashing
Install-Package BCrypt.Net-Next -Version 4.0.3

# Optional: AutoMapper for DTO mapping
Install-Package AutoMapper -Version 12.0.1
```

**Deliverable**: All required NuGet packages installed

---

### Step 1.5: Configure Entity Framework
**Duration**: 1 hour

**Create: `Models/Entities/User.cs`**
```csharp
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models.Entities
{
    [Table("Users")]
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Index(IsUnique = true)]
        public string Username { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        [Index(IsUnique = true)]
        public string Email { get; set; }

        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; }

        [Required]
        [StringLength(20)]
        public string Role { get; set; } = "Customer"; // Admin, Customer, Guest

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(20)]
        public string PhoneNumber { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual ICollection<Order> Orders { get; set; }
        public virtual Cart Cart { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
```

**Create: `Models/Entities/Product.cs`**
```csharp
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models.Entities
{
    [Table("Products")]
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        [Range(0.01, 10000)]
        public decimal Price { get; set; }

        public int? CategoryId { get; set; }

        [StringLength(255)]
        public string ImageUrl { get; set; }

        [Range(0, int.MaxValue)]
        public int Stock { get; set; } = 0;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public virtual ICollection<CartItem> CartItems { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
```

**Create: `Models/ApplicationDbContext.cs`**
```csharp
using System.Data.Entity;
using ECommerce.Models.Entities;
using MySql.Data.EntityFramework;

namespace ECommerce.Models
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("DefaultConnection")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships and constraints here
        }
    }
}
```

**Update: `Web.config`**
```xml
<connectionStrings>
  <add name="DefaultConnection" 
       connectionString="server=localhost;port=3306;database=ecommerce_db;uid=root;password=YOUR_PASSWORD;SslMode=none;"
       providerName="MySql.Data.MySqlClient" />
</connectionStrings>
```

**Deliverable**: Entity Framework configured with DbContext and connection string

---

## Phase 2: Authentication & Authorization (Days 4-6)

### Step 2.1: Create Authentication Service
**Duration**: 2 hours

**Create: `Services/AuthService.cs`**
```csharp
using System;
using System.Linq;
using ECommerce.Models;
using ECommerce.Models.Entities;
using BCrypt.Net;

namespace ECommerce.Services
{
    public class AuthService
    {
        private readonly ApplicationDbContext _db;

        public AuthService()
        {
            _db = new ApplicationDbContext();
        }

        public User Register(string username, string email, string password, string firstName, string lastName)
        {
            // Check if user exists
            if (_db.Users.Any(u => u.Email == email || u.Username == username))
            {
                throw new Exception("User already exists");
            }

            // Hash password
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            // Create user
            var user = new User
            {
                Username = username,
                Email = email,
                PasswordHash = passwordHash,
                FirstName = firstName,
                LastName = lastName,
                Role = "Customer",
                IsActive = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _db.Users.Add(user);
            _db.SaveChanges();

            return user;
        }

        public User Login(string email, string password)
        {
            var user = _db.Users.FirstOrDefault(u => u.Email == email && u.IsActive);

            if (user == null)
            {
                throw new Exception("Invalid email or password");
            }

            // Verify password
            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                throw new Exception("Invalid email or password");
            }

            return user;
        }

        public bool IsUserInRole(int userId, string role)
        {
            var user = _db.Users.Find(userId);
            return user != null && user.Role == role;
        }
    }
}
```

**Deliverable**: Authentication service with registration and login

---

### Step 2.2: Create Account Controller
**Duration**: 2 hours

**Create: `Controllers/AccountController.cs`**
```csharp
using System;
using System.Web;
using System.Web.Mvc;
using ECommerce.Models.ViewModels;
using ECommerce.Services;

namespace ECommerce.Controllers
{
    public class AccountController : Controller
    {
        private readonly AuthService _authService;

        public AccountController()
        {
            _authService = new AuthService();
        }

        // GET: Account/Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var user = _authService.Register(
                    model.Username,
                    model.Email,
                    model.Password,
                    model.FirstName,
                    model.LastName
                );

                // Auto login after registration
                Session["UserId"] = user.Id;
                Session["Username"] = user.Username;
                Session["UserRole"] = user.Role;

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        // GET: Account/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var user = _authService.Login(model.Email, model.Password);

                Session["UserId"] = user.Id;
                Session["Username"] = user.Username;
                Session["UserRole"] = user.Role;

                if (user.Role == "Admin")
                {
                    return RedirectToAction("Index", "Admin");
                }

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        // GET: Account/Logout
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
```

**Deliverable**: Account controller with registration, login, and logout

---

### Step 2.3: Create Authorization Attributes
**Duration**: 1 hour

**Create: `Filters/CustomAuthorizeAttribute.cs`**
```csharp
using System;
using System.Web;
using System.Web.Mvc;

namespace ECommerce.Filters
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        public string RequiredRole { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Session["UserId"] == null)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(RequiredRole))
            {
                var userRole = httpContext.Session["UserRole"]?.ToString();
                return userRole == RequiredRole;
            }

            return true;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Session["UserId"] == null)
            {
                filterContext.Result = new RedirectResult("/Account/Login");
            }
            else
            {
                filterContext.Result = new RedirectResult("/Home/Unauthorized");
            }
        }
    }
}
```

**Deliverable**: Custom authorization attribute for role-based access control

---

## Phase 3: Product Management (Days 7-10)

### Step 3.1: Create Product Service
**Duration**: 2 hours

**Create: `Services/ProductService.cs`**
```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using ECommerce.Models;
using ECommerce.Models.Entities;

namespace ECommerce.Services
{
    public class ProductService
    {
        private readonly ApplicationDbContext _db;

        public ProductService()
        {
            _db = new ApplicationDbContext();
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _db.Products
                .Include(p => p.Category)
                .Where(p => p.IsActive)
                .ToList();
        }

        public Product GetProductById(int id)
        {
            return _db.Products
                .Include(p => p.Category)
                .FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Product> SearchProducts(string searchTerm)
        {
            return _db.Products
                .Include(p => p.Category)
                .Where(p => p.IsActive && p.Name.Contains(searchTerm))
                .ToList();
        }

        public IEnumerable<Product> GetProductsByCategory(int categoryId)
        {
            return _db.Products
                .Include(p => p.Category)
                .Where(p => p.IsActive && p.CategoryId == categoryId)
                .ToList();
        }

        public void CreateProduct(Product product)
        {
            product.CreatedAt = DateTime.Now;
            product.UpdatedAt = DateTime.Now;
            _db.Products.Add(product);
            _db.SaveChanges();
        }

        public void UpdateProduct(Product product)
        {
            product.UpdatedAt = DateTime.Now;
            _db.Entry(product).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public void DeleteProduct(int id)
        {
            var product = _db.Products.Find(id);
            if (product != null)
            {
                product.IsActive = false; // Soft delete
                product.UpdatedAt = DateTime.Now;
                _db.SaveChanges();
            }
        }

        public bool IsInStock(int productId, int quantity)
        {
            var product = _db.Products.Find(productId);
            return product != null && product.Stock >= quantity;
        }

        public void UpdateStock(int productId, int quantity)
        {
            var product = _db.Products.Find(productId);
            if (product != null)
            {
                product.Stock -= quantity;
                product.UpdatedAt = DateTime.Now;
                _db.SaveChanges();
            }
        }
    }
}
```

**Deliverable**: Product service with full CRUD operations

---

### Step 3.2: Update Home Controller
**Duration**: 1 hour

**Update: `Controllers/HomeController.cs`**
```csharp
using System.Web.Mvc;
using ECommerce.Services;

namespace ECommerce.Controllers
{
    public class HomeController : Controller
    {
        private readonly ProductService _productService;

        public HomeController()
        {
            _productService = new ProductService();
        }

        public ActionResult Index(string search, int? categoryId)
        {
            var products = string.IsNullOrEmpty(search)
                ? (categoryId.HasValue
                    ? _productService.GetProductsByCategory(categoryId.Value)
                    : _productService.GetAllProducts())
                : _productService.SearchProducts(search);

            return View(products);
        }
    }
}
```

**Deliverable**: Updated home controller using product service

---

### Step 3.3: Create Admin Product Controller
**Duration**: 3 hours

**Create: `Controllers/AdminProductController.cs`**
```csharp
using System.Web.Mvc;
using ECommerce.Filters;
using ECommerce.Models.Entities;
using ECommerce.Models.ViewModels;
using ECommerce.Services;

namespace ECommerce.Controllers
{
    [CustomAuthorize(RequiredRole = "Admin")]
    public class AdminProductController : Controller
    {
        private readonly ProductService _productService;

        public AdminProductController()
        {
            _productService = new ProductService();
        }

        // GET: AdminProduct
        public ActionResult Index()
        {
            var products = _productService.GetAllProducts();
            return View(products);
        }

        // GET: AdminProduct/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminProduct/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var product = new Product
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                CategoryId = model.CategoryId,
                ImageUrl = model.ImageUrl,
                Stock = model.Stock,
                IsActive = true
            };

            _productService.CreateProduct(product);
            return RedirectToAction("Index");
        }

        // GET: AdminProduct/Edit/5
        public ActionResult Edit(int id)
        {
            var product = _productService.GetProductById(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            var model = new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryId = product.CategoryId,
                ImageUrl = product.ImageUrl,
                Stock = product.Stock
            };

            return View(model);
        }

        // POST: AdminProduct/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var product = _productService.GetProductById(model.Id);
            if (product == null)
            {
                return HttpNotFound();
            }

            product.Name = model.Name;
            product.Description = model.Description;
            product.Price = model.Price;
            product.CategoryId = model.CategoryId;
            product.ImageUrl = model.ImageUrl;
            product.Stock = model.Stock;

            _productService.UpdateProduct(product);
            return RedirectToAction("Index");
        }

        // POST: AdminProduct/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            _productService.DeleteProduct(id);
            return RedirectToAction("Index");
        }
    }
}
```

**Deliverable**: Admin product controller with full CRUD operations

---

## Phase 4: Shopping Cart & Checkout (Days 11-14)

### Step 4.1: Create Cart Service
**Duration**: 2 hours

**Create: `Services/CartService.cs`**
```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using ECommerce.Models;
using ECommerce.Models.Entities;

namespace ECommerce.Services
{
    public class CartService
    {
        // Using session-based cart (alternatively can use database)
        public class CartItemDTO
        {
            public int ProductId { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }
            public int Quantity { get; set; }
            public string ImageUrl { get; set; }
            public decimal Subtotal => Price * Quantity;
        }

        public List<CartItemDTO> GetCart(System.Web.HttpSessionStateBase session)
        {
            var cart = session["Cart"] as List<CartItemDTO>;
            if (cart == null)
            {
                cart = new List<CartItemDTO>();
                session["Cart"] = cart;
            }
            return cart;
        }

        public void AddToCart(System.Web.HttpSessionStateBase session, Product product, int quantity)
        {
            var cart = GetCart(session);
            var existingItem = cart.FirstOrDefault(c => c.ProductId == product.Id);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cart.Add(new CartItemDTO
                {
                    ProductId = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Quantity = quantity,
                    ImageUrl = product.ImageUrl
                });
            }

            session["Cart"] = cart;
        }

        public void RemoveFromCart(System.Web.HttpSessionStateBase session, int productId)
        {
            var cart = GetCart(session);
            var item = cart.FirstOrDefault(c => c.ProductId == productId);
            if (item != null)
            {
                cart.Remove(item);
            }
            session["Cart"] = cart;
        }

        public void UpdateQuantity(System.Web.HttpSessionStateBase session, int productId, int quantity)
        {
            var cart = GetCart(session);
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
                }
            }
            session["Cart"] = cart;
        }

        public void ClearCart(System.Web.HttpSessionStateBase session)
        {
            session["Cart"] = new List<CartItemDTO>();
        }

        public decimal GetCartTotal(System.Web.HttpSessionStateBase session)
        {
            var cart = GetCart(session);
            return cart.Sum(c => c.Subtotal);
        }

        public int GetCartItemCount(System.Web.HttpSessionStateBase session)
        {
            var cart = GetCart(session);
            return cart.Sum(c => c.Quantity);
        }
    }
}
```

**Deliverable**: Cart service with session-based cart management

---

### Step 4.2: Create Cart Controller
**Duration**: 2 hours

**Create: `Controllers/CartController.cs`**
```csharp
using System.Web.Mvc;
using ECommerce.Services;

namespace ECommerce.Controllers
{
    public class CartController : Controller
    {
        private readonly CartService _cartService;
        private readonly ProductService _productService;

        public CartController()
        {
            _cartService = new CartService();
            _productService = new ProductService();
        }

        // GET: Cart
        public ActionResult Index()
        {
            var cart = _cartService.GetCart(Session);
            ViewBag.Total = _cartService.GetCartTotal(Session);
            return View(cart);
        }

        // POST: Cart/Add
        [HttpPost]
        public ActionResult Add(int productId, int quantity = 1)
        {
            var product = _productService.GetProductById(productId);
            if (product == null)
            {
                return HttpNotFound();
            }

            if (!_productService.IsInStock(productId, quantity))
            {
                TempData["Error"] = "Insufficient stock";
                return RedirectToAction("Index", "Home");
            }

            _cartService.AddToCart(Session, product, quantity);
            TempData["Success"] = "Item added to cart";
            return RedirectToAction("Index");
        }

        // POST: Cart/Remove
        [HttpPost]
        public ActionResult Remove(int productId)
        {
            _cartService.RemoveFromCart(Session, productId);
            return RedirectToAction("Index");
        }

        // POST: Cart/Update
        [HttpPost]
        public ActionResult Update(int productId, int quantity)
        {
            _cartService.UpdateQuantity(Session, productId, quantity);
            return RedirectToAction("Index");
        }

        // GET: Cart/ItemCount (for AJAX)
        public JsonResult ItemCount()
        {
            var count = _cartService.GetCartItemCount(Session);
            return Json(new { count }, JsonRequestBehavior.AllowGet);
        }
    }
}
```

**Deliverable**: Cart controller with add, remove, update operations

---

### Step 4.3: Create Order Service
**Duration**: 2 hours

**Create: `Services/OrderService.cs`**
```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using ECommerce.Models;
using ECommerce.Models.Entities;

namespace ECommerce.Services
{
    public class OrderService
    {
        private readonly ApplicationDbContext _db;
        private readonly ProductService _productService;

        public OrderService()
        {
            _db = new ApplicationDbContext();
            _productService = new ProductService();
        }

        public Order CreateOrder(int userId, List<CartService.CartItemDTO> cartItems, string shippingAddress)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    // Create order
                    var order = new Order
                    {
                        UserId = userId,
                        OrderDate = DateTime.Now,
                        Status = "Pending",
                        ShippingAddress = shippingAddress,
                        PaymentMethod = "Simulated",
                        TotalAmount = cartItems.Sum(c => c.Subtotal),
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };

                    _db.Orders.Add(order);
                    _db.SaveChanges();

                    // Create order items
                    foreach (var item in cartItems)
                    {
                        var orderItem = new OrderItem
                        {
                            OrderId = order.Id,
                            ProductId = item.ProductId,
                            Quantity = item.Quantity,
                            UnitPrice = item.Price,
                            Subtotal = item.Subtotal
                        };

                        _db.OrderItems.Add(orderItem);

                        // Update stock
                        _productService.UpdateStock(item.ProductId, item.Quantity);
                    }

                    _db.SaveChanges();
                    transaction.Commit();

                    return order;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public IEnumerable<Order> GetUserOrders(int userId)
        {
            return _db.Orders
                .Include(o => o.OrderItems.Select(oi => oi.Product))
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToList();
        }

        public IEnumerable<Order> GetAllOrders()
        {
            return _db.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems.Select(oi => oi.Product))
                .OrderByDescending(o => o.OrderDate)
                .ToList();
        }

        public Order GetOrderById(int orderId)
        {
            return _db.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems.Select(oi => oi.Product))
                .FirstOrDefault(o => o.Id == orderId);
        }

        public void UpdateOrderStatus(int orderId, string status)
        {
            var order = _db.Orders.Find(orderId);
            if (order != null)
            {
                order.Status = status;
                order.UpdatedAt = DateTime.Now;
                _db.SaveChanges();
            }
        }
    }
}
```

**Deliverable**: Order service with order creation and management

---

### Step 4.4: Create Checkout Controller
**Duration**: 2 hours

**Create: `Controllers/CheckoutController.cs`**
```csharp
using System;
using System.Web.Mvc;
using ECommerce.Filters;
using ECommerce.Models.ViewModels;
using ECommerce.Services;

namespace ECommerce.Controllers
{
    [CustomAuthorize]
    public class CheckoutController : Controller
    {
        private readonly CartService _cartService;
        private readonly OrderService _orderService;

        public CheckoutController()
        {
            _cartService = new CartService();
            _orderService = new OrderService();
        }

        // GET: Checkout
        public ActionResult Index()
        {
            var cart = _cartService.GetCart(Session);
            if (cart.Count == 0)
            {
                return RedirectToAction("Index", "Cart");
            }

            ViewBag.Total = _cartService.GetCartTotal(Session);
            return View(cart);
        }

        // POST: Checkout/Process
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Process(CheckoutViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            try
            {
                var userId = (int)Session["UserId"];
                var cart = _cartService.GetCart(Session);

                if (cart.Count == 0)
                {
                    return RedirectToAction("Index", "Cart");
                }

                var order = _orderService.CreateOrder(userId, cart, model.ShippingAddress);

                _cartService.ClearCart(Session);

                return RedirectToAction("Confirmation", new { orderId = order.Id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error processing order: " + ex.Message);
                return View("Index", model);
            }
        }

        // GET: Checkout/Confirmation/5
        public ActionResult Confirmation(int orderId)
        {
            var order = _orderService.GetOrderById(orderId);
            if (order == null)
            {
                return HttpNotFound();
            }

            return View(order);
        }
    }
}
```

**Deliverable**: Checkout controller with order processing

---

## Phase 5: Admin Dashboard (Days 15-17)

### Step 5.1: Create Admin Dashboard Controller
**Duration**: 3 hours

**Create: `Controllers/AdminController.cs`**
```csharp
using System;
using System.Linq;
using System.Web.Mvc;
using ECommerce.Filters;
using ECommerce.Models;
using ECommerce.Models.ViewModels;
using ECommerce.Services;

namespace ECommerce.Controllers
{
    [CustomAuthorize(RequiredRole = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly OrderService _orderService;
        private readonly ProductService _productService;

        public AdminController()
        {
            _db = new ApplicationDbContext();
            _orderService = new OrderService();
            _productService = new ProductService();
        }

        // GET: Admin
        public ActionResult Index()
        {
            var model = new AdminDashboardViewModel
            {
                TotalOrders = _db.Orders.Count(),
                TotalRevenue = _db.Orders.Sum(o => (decimal?)o.TotalAmount) ?? 0,
                TotalCustomers = _db.Users.Count(u => u.Role == "Customer"),
                TotalProducts = _db.Products.Count(p => p.IsActive),
                RecentOrders = _orderService.GetAllOrders().Take(10).ToList(),
                LowStockProducts = _db.Products.Where(p => p.Stock < 20 && p.IsActive).ToList()
            };

            return View(model);
        }

        // GET: Admin/Orders
        public ActionResult Orders(string status)
        {
            var orders = string.IsNullOrEmpty(status)
                ? _orderService.GetAllOrders()
                : _orderService.GetAllOrders().Where(o => o.Status == status);

            return View(orders);
        }

        // GET: Admin/OrderDetails/5
        public ActionResult OrderDetails(int id)
        {
            var order = _orderService.GetOrderById(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Admin/UpdateOrderStatus
        [HttpPost]
        public ActionResult UpdateOrderStatus(int orderId, string status)
        {
            _orderService.UpdateOrderStatus(orderId, status);
            return RedirectToAction("OrderDetails", new { id = orderId });
        }

        // GET: Admin/Customers
        public ActionResult Customers()
        {
            var customers = _db.Users.Where(u => u.Role == "Customer").ToList();
            return View(customers);
        }

        // POST: Admin/DeactivateCustomer
        [HttpPost]
        public ActionResult DeactivateCustomer(int userId)
        {
            var user = _db.Users.Find(userId);
            if (user != null)
            {
                user.IsActive = false;
                user.UpdatedAt = DateTime.Now;
                _db.SaveChanges();
            }
            return RedirectToAction("Customers");
        }

        // POST: Admin/ActivateCustomer
        [HttpPost]
        public ActionResult ActivateCustomer(int userId)
        {
            var user = _db.Users.Find(userId);
            if (user != null)
            {
                user.IsActive = true;
                user.UpdatedAt = DateTime.Now;
                _db.SaveChanges();
            }
            return RedirectToAction("Customers");
        }

        // GET: Admin/SalesReport
        public ActionResult SalesReport(DateTime? startDate, DateTime? endDate)
        {
            var orders = _db.Orders.AsQueryable();

            if (startDate.HasValue)
            {
                orders = orders.Where(o => o.OrderDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                orders = orders.Where(o => o.OrderDate <= endDate.Value);
            }

            var model = new SalesReportViewModel
            {
                StartDate = startDate,
                EndDate = endDate,
                TotalOrders = orders.Count(),
                TotalRevenue = orders.Sum(o => (decimal?)o.TotalAmount) ?? 0,
                AverageOrderValue = orders.Average(o => (decimal?)o.TotalAmount) ?? 0,
                OrdersByStatus = orders.GroupBy(o => o.Status)
                    .Select(g => new { Status = g.Key, Count = g.Count() })
                    .ToDictionary(x => x.Status, x => x.Count)
            };

            return View(model);
        }
    }
}
```

**Deliverable**: Admin dashboard controller with analytics and management functions

---

## Phase 6: Security & Validation (Days 18-19)

### Step 6.1: Implement CSRF Protection
**Duration**: 1 hour

**Add to all forms**:
```cshtml
@using (Html.BeginForm("ActionName", "ControllerName", FormMethod.Post))
{
    @Html.AntiForgeryToken()
    <!-- form fields -->
}
```

**Verify in controllers**:
```csharp
[HttpPost]
[ValidateAntiForgeryToken]
public ActionResult ActionName(ModelType model)
{
    // method body
}
```

**Deliverable**: CSRF tokens in all forms

---

### Step 6.2: Implement Server-Side Validation
**Duration**: 2 hours

**Create ViewModels with validation**:
```csharp
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, ErrorMessage = "Username must be between 3 and 50 characters", MinimumLength = 3)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, ErrorMessage = "Password must be at least 6 characters", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please confirm your password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50)]
        public string LastName { get; set; }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
```

**Deliverable**: ViewModels with comprehensive validation attributes

---

### Step 6.3: Configure Secure Sessions
**Duration**: 1 hour

**Update Web.config**:
```xml
<system.web>
  <sessionState mode="InProc" timeout="30" />
  <httpCookies httpOnlyCookies="true" requireSSL="true" />
  <authentication mode="Forms">
    <forms loginUrl="~/Account/Login" 
           timeout="30" 
           slidingExpiration="true"
           requireSSL="true"
           cookieless="UseCookies" />
  </authentication>
</system.web>
```

**Deliverable**: Secure session configuration

---

## Phase 7: Views & UI Integration (Days 20-22)

### Step 7.1: Update Existing Views
**Duration**: 4 hours

- Update `Home/Index.cshtml` to use database products
- Create `Account/Login.cshtml` and `Account/Register.cshtml`
- Create `Cart/Index.cshtml`
- Create `Checkout/Index.cshtml` and `Checkout/Confirmation.cshtml`

### Step 7.2: Create Admin Views
**Duration**: 4 hours

- Create `Admin/Index.cshtml` (dashboard)
- Create `AdminProduct/Index.cshtml`, `Create.cshtml`, `Edit.cshtml`
- Create `Admin/Orders.cshtml`, `OrderDetails.cshtml`
- Create `Admin/Customers.cshtml`
- Create `Admin/SalesReport.cshtml`

### Step 7.3: Update Layout
**Duration**: 1 hour

**Update `Views/Shared/_Layout.cshtml`**:
- Add user info display
- Add logout button
- Add cart item count
- Add admin link (if admin role)

---

## Phase 8: Testing & Polish (Days 23-25)

### Step 8.1: Functional Testing
- Test user registration and login
- Test product CRUD (admin)
- Test shopping cart operations
- Test checkout process
- Test order management
- Test customer management

### Step 8.2: Security Testing
- Verify SQL injection prevention (EF handles this)
- Test XSS prevention
- Verify CSRF protection
- Test authorization (role-based access)
- Verify password hashing

### Step 8.3: UI/UX Polish
- Ensure responsive design works
- Add loading indicators
- Add error messages
- Add success notifications
- Improve form validation messages

---

## Bonus Features (Optional)

### Product Reviews
- Implement review model
- Add review submission
- Display reviews on product page
- Calculate average ratings

### Email Notifications
- Install MailKit NuGet package
- Configure SMTP settings
- Send order confirmation emails
- Send registration welcome emails

### Admin Analytics
- Add Chart.js for visualizations
- Sales trends graph
- Top products chart
- Customer growth chart

### Inventory Alerts
- Email admin when stock is low
- Dashboard notifications
- Automatic stock tracking

---

## Success Criteria Checklist

### Technical Requirements
- [x] ASP.NET MVC Framework (not Core)
- [ ] MySQL database integration
- [ ] Entity Framework configured
- [ ] At least 3 user roles (Admin, Customer, Guest)
- [ ] User authentication implemented
- [ ] Role-based authorization implemented
- [ ] CRUD for Products
- [ ] CRUD for Orders
- [ ] CRUD for Users
- [ ] Client-side form validation
- [ ] Server-side form validation
- [ ] Session management (cart and login)
- [ ] Responsive design (Bootstrap)
- [ ] SQL injection prevention (EF)
- [ ] Password hashing (BCrypt)

### Admin Features
- [ ] Admin login
- [ ] Admin dashboard with stats
- [ ] Product management (add/edit/delete/view)
- [ ] Order management (view/update status)
- [ ] Customer management (view/deactivate)
- [ ] Sales reports

### Customer Features
- [ ] Registration and login
- [ ] Browse products
- [ ] Search products
- [ ] Add to cart
- [ ] Checkout (simulated payment)
- [ ] View order history

### Guest Features
- [ ] Browse products (limited)
- [ ] Register/login prompt on checkout

---

## Timeline Summary

| Phase | Days | Description |
|-------|------|-------------|
| 1 | 1-3 | Database setup and Entity Framework |
| 2 | 4-6 | Authentication and authorization |
| 3 | 7-10 | Product management (CRUD) |
| 4 | 11-14 | Cart and checkout |
| 5 | 15-17 | Admin dashboard |
| 6 | 18-19 | Security and validation |
| 7 | 20-22 | Views and UI integration |
| 8 | 23-25 | Testing and polish |
| Bonus | 26-30 | Optional features |

**Total**: 25-30 days for complete implementation

---

## Key Commands Reference

### Create Database
```sql
CREATE DATABASE ecommerce_db;
USE ecommerce_db;
```

### Run Migrations (if using Code-First)
```powershell
Enable-Migrations
Add-Migration InitialCreate
Update-Database
```

### Build Solution
```powershell
dotnet build
# or in Visual Studio: Ctrl+Shift+B
```

### Run Application
```powershell
# In Visual Studio: F5
# or IIS Express from command line
```

---

## Important Notes

1. **Connection String**: Update MySQL password in Web.config
2. **Image Upload**: Consider implementing file upload for product images
3. **Payment Gateway**: Currently simulated; can integrate real gateway later
4. **Email Service**: Configure SMTP for email notifications
5. **Logging**: Consider adding logging (e.g., NLog) for debugging
6. **Error Handling**: Implement global error handling
7. **Performance**: Add caching for frequently accessed data
8. **Backup**: Regular database backups recommended

---

## Resources

- **Entity Framework**: https://docs.microsoft.com/en-us/ef/
- **MySQL Connector**: https://dev.mysql.com/doc/connector-net/en/
- **BCrypt.Net**: https://github.com/BcryptNet/bcrypt.net
- **Bootstrap**: https://getbootstrap.com/
- **jQuery Validation**: https://jqueryvalidation.org/

---

**End of Implementation Plan**
