# E-Commerce Web Application

## 🎯 Project Overview

A full-featured e-commerce web application built with **ASP.NET MVC** and **MySQL** demonstrating practical implementation of MVC architecture, database integration, authentication, authorization, and web development best practices.

### Key Features
- 🛒 Complete shopping cart system
- 👤 User authentication & role-based authorization (Admin, Customer, Guest)
- 📦 Product catalog with categories and search
- 💳 Checkout process with order management
- 📊 Admin dashboard with analytics
- 🔒 Security best practices (password hashing, CSRF protection, input validation)

---

## 📸 Architecture Diagram

![E-Commerce Architecture](../.gemini/antigravity/brain/59512f79-ec4c-43d6-900e-3c5820e3d502/ecommerce_architecture_diagram_1763889835485.png)

---

## 🏗️ Technology Stack

### Backend
- **Framework**: ASP.NET MVC 5.2.9 (.NET Framework 4.7.2)
- **Database**: MySQL 8.0+
- **ORM**: Entity Framework 6.4.4
- **Authentication**: Custom (BCrypt password hashing)

### Frontend
- **UI Framework**: Bootstrap 5.2.3
- **JavaScript**: jQuery 3.7.0
- **Validation**: jQuery Validation

### Security
- BCrypt.Net-Next 4.0.3 (password hashing)
- Anti-Forgery Tokens (CSRF protection)
- Role-based authorization
- Secure session management

---

## 📂 Project Structure

```
ECommerce/
├── Controllers/          # MVC Controllers
│   ├── AccountController.cs
│   ├── ProductController.cs
│   ├── CartController.cs
│   ├── CheckoutController.cs
│   ├── AdminController.cs
│   └── AdminProductController.cs
├── Models/              # Data models
│   ├── Entities/        # Database entities
│   │   ├── User.cs
│   │   ├── Product.cs
│   │   ├── Order.cs
│   │   └── ...
│   ├── ViewModels/      # View models
│   │   ├── LoginViewModel.cs
│   │   ├── RegisterViewModel.cs
│   │   └── ...
│   └── ApplicationDbContext.cs
├── Views/               # Razor views
│   ├── Home/
│   ├── Account/
│   ├── Cart/
│   ├── Checkout/
│   └── Admin/
├── Services/            # Business logic
│   ├── AuthService.cs
│   ├── ProductService.cs
│   ├── CartService.cs
│   └── OrderService.cs
├── Filters/             # Custom filters
│   └── CustomAuthorizeAttribute.cs
├── Content/             # Static assets
│   ├── CSS files
│   ├── Images
│   └── Bootstrap
└── Scripts/             # JavaScript files
```

---

## 🗄️ Database Schema

### Core Tables
- **Users**: User accounts with roles (Admin, Customer)
- **Products**: Product catalog with pricing and inventory
- **Categories**: Product categorization (Leafy, Root, Fruit vegetables)
- **Orders**: Customer orders with status tracking
- **OrderItems**: Individual items in each order
- **Carts**: Shopping cart persistence
- **CartItems**: Items in shopping carts
- **Reviews**: Product reviews and ratings (bonus)

---

## 🚀 Getting Started

### Prerequisites
- Visual Studio 2019 or later
- MySQL Server 8.0+
- .NET Framework 4.7.2

### Installation Steps

1. **Clone the repository**
   ```bash
   git clone https://github.com/jayjayandcattos/MVC-Ecommerce-IPT.git
   cd MVC-Ecommerce-IPT
   ```

2. **Setup MySQL Database**
   ```sql
   CREATE DATABASE ecommerce_db;
   USE ecommerce_db;
   -- Run schema from .agent/workflows/implementation_plan.md (Phase 1, Step 1.2)
   ```

3. **Install NuGet Packages**
   ```powershell
   cd ECommerce
   Install-Package EntityFramework -Version 6.4.4
   Install-Package MySql.Data -Version 8.0.33
   Install-Package MySql.Data.EntityFramework -Version 8.0.33
   Install-Package BCrypt.Net-Next -Version 4.0.3
   ```

4. **Update Connection String**
   
   Edit `Web.config`:
   ```xml
   <connectionStrings>
     <add name="DefaultConnection" 
          connectionString="server=localhost;port=3306;database=ecommerce_db;uid=root;password=YOUR_PASSWORD;SslMode=none;"
          providerName="MySql.Data.MySqlClient" />
   </connectionStrings>
   ```

5. **Build and Run**
   - Open `ECommerce.sln` in Visual Studio
   - Press **F5** to build and run

---

## 👥 User Roles

### Admin
- Full access to admin dashboard
- Manage products (CRUD operations)
- Manage orders and update status
- View customer accounts
- Generate sales reports

**Default Admin Credentials**:
- Email: `admin@ecommerce.com`
- Password: `Admin123!`

### Customer
- Register and login
- Browse and search products
- Add items to shopping cart
- Complete checkout process
- View order history

### Guest
- Browse products (read-only)
- Must register to add items to cart
- Prompted to login at checkout

---

## 🔐 Security Features

✅ **Password Hashing**: BCrypt with salt  
✅ **SQL Injection Prevention**: Entity Framework parameterized queries  
✅ **XSS Protection**: Razor @Html encoding  
✅ **CSRF Protection**: Anti-forgery tokens in all forms  
✅ **Role-Based Authorization**: Custom authorize attributes  
✅ **Secure Sessions**: HttpOnly cookies, session timeout  
✅ **Input Validation**: Client and server-side validation  

---

## 📋 Features Implemented

### ✅ Core Features
- [x] User registration and authentication
- [x] Role-based authorization (Admin, Customer, Guest)
- [x] Product catalog with categories
- [x] Product search and filtering
- [x] Shopping cart (session-based)
- [x] Checkout process with order placement
- [x] Order history for customers
- [x] Admin dashboard with statistics
- [x] Product management (CRUD)
- [x] Order management with status updates
- [x] Customer account management
- [x] Sales reporting

### 🎁 Bonus Features (Optional)
- [ ] Product reviews and ratings
- [ ] Email notifications (order confirmation, welcome emails)
- [ ] Advanced analytics dashboard
- [ ] Inventory management with alerts
- [ ] Product image upload
- [ ] Wishlist functionality

---

## 🧪 Testing

### Manual Testing Checklist

**Authentication**:
- [ ] User registration with validation
- [ ] User login with correct/incorrect credentials
- [ ] Session persistence across pages
- [ ] Logout functionality

**Shopping Flow**:
- [ ] Browse products as guest
- [ ] Search and filter products
- [ ] Add items to cart
- [ ] Update cart quantities
- [ ] Remove items from cart
- [ ] Checkout process (requires login)
- [ ] Order confirmation

**Admin Functions**:
- [ ] Access admin dashboard (admin only)
- [ ] Create new product
- [ ] Edit existing product
- [ ] Delete product (soft delete)
- [ ] View all orders
- [ ] Update order status
- [ ] View customer list
- [ ] Generate sales report

**Security**:
- [ ] Non-admin cannot access admin pages
- [ ] Guest redirected to login at checkout
- [ ] CSRF tokens present in forms
- [ ] Passwords hashed in database
- [ ] Input validation working

---

## 📊 Project Statistics

| Metric | Value |
|--------|-------|
| **Lines of Code** | ~5,000+ (estimated at completion) |
| **Controllers** | 7 |
| **Models** | 8 entities + ViewModels |
| **Services** | 4 |
| **Views** | 20+ |
| **Database Tables** | 8 |

---

## 📚 Documentation

- **[Project Analysis](../.agent/analysis/project_analysis.md)**: Detailed analysis of current state and requirements
- **[Implementation Plan](../.agent/workflows/implementation_plan.md)**: Phase-by-phase development plan with code examples
- **[Quick Reference](../.agent/QUICK_REFERENCE.md)**: Commands, checklists, and troubleshooting guide

---

## 🛠️ Development Timeline

| Phase | Duration | Description |
|-------|----------|-------------|
| Phase 1 | 3 days | Database setup & Entity Framework |
| Phase 2 | 3 days | Authentication & authorization |
| Phase 3 | 4 days | Product management |
| Phase 4 | 4 days | Shopping cart & checkout |
| Phase 5 | 3 days | Admin dashboard |
| Phase 6 | 2 days | Security & validation |
| Phase 7 | 3 days | Views & UI integration |
| Phase 8 | 3 days | Testing & polish |
| **Total** | **25 days** | Core features complete |

---

## 🐛 Known Issues

Currently in development. Check [Issues](../../issues) for active bugs and feature requests.

---

## 🤝 Contributing

This is a learning project. Contributions, issues, and feature requests are welcome!

1. Fork the project
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

---

## 📝 License

This project is for educational purposes.

---

## 👨‍💻 Author

**Team: jayjayandcattos**

---

## 🙏 Acknowledgments

- ASP.NET MVC documentation
- Entity Framework documentation
- MySQL Connector/NET documentation
- Bootstrap framework
- Stack Overflow community

---

## 📧 Contact

For questions or support, please open an issue in the repository.

---

**Last Updated**: November 23, 2025  
**Version**: 1.0  
**Status**: In Active Development 🚧