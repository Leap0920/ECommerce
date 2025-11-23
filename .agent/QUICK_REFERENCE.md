# E-Commerce Project - Quick Reference Guide

## 📊 Project Status

**Current Completion**: ~18%

**What Works**:
- ✅ Frontend product display (JSON-based)
- ✅ Client-side search and filtering
- ✅ Shopping cart (LocalStorage only)
- ✅ Responsive UI with Bootstrap

**What's Missing**:
- ❌ MySQL database (critical)
- ❌ Backend authentication
- ❌ Product CRUD operations
- ❌ Order management
- ❌ Admin dashboard
- ❌ Security implementation

---

## 🚀 Quick Start Commands

### 1. Setup MySQL Database
```sql
CREATE DATABASE ecommerce_db;
USE ecommerce_db;
-- Then run schema from implementation_plan.md (Phase 1, Step 1.2)
```

### 2. Install Required NuGet Packages
```powershell
cd c:\Users\Justin\source\repos\MVC-Ecommerce-IPT\ECommerce

Install-Package EntityFramework -Version 6.4.4
Install-Package MySql.Data -Version 8.0.33
Install-Package MySql.Data.EntityFramework -Version 8.0.33
Install-Package BCrypt.Net-Next -Version 4.0.3
```

### 3. Update Web.config Connection String
```xml
<connectionStrings>
  <add name="DefaultConnection" 
       connectionString="server=localhost;port=3306;database=ecommerce_db;uid=root;password=YOUR_PASSWORD;SslMode=none;"
       providerName="MySql.Data.MySqlClient" />
</connectionStrings>
```

### 4. Build and Run
Press **F5** in Visual Studio or use:
```powershell
msbuild ECommerce.sln
```

---

## 📁 Project Structure

```
ECommerce/
├── App_Start/           # Configuration (Routing, Bundles, Filters)
├── Content/             # CSS, Images, JSON (products)
├── Controllers/         # MVC Controllers
│   ├── HomeController.cs          (✅ Basic implementation)
│   ├── RegisController.cs         (⚠️ Empty templates)
│   ├── CheckedOutController.cs    (⚠️ Basic)
│   └── [TO CREATE]:
│       ├── AccountController.cs   (Login/Register)
│       ├── ProductController.cs   (Browse products)
│       ├── CartController.cs      (Shopping cart)
│       ├── CheckoutController.cs  (Order placement)
│       ├── AdminController.cs     (Dashboard)
│       └── AdminProductController.cs (Product CRUD)
├── Models/              # ❌ EMPTY - CRITICAL GAP
│   └── [TO CREATE]:
│       ├── ApplicationDbContext.cs
│       ├── Entities/
│       │   ├── User.cs
│       │   ├── Product.cs
│       │   ├── Order.cs
│       │   ├── OrderItem.cs
│       │   ├── Cart.cs
│       │   ├── CartItem.cs
│       │   ├── Category.cs
│       │   └── Review.cs
│       └── ViewModels/
│           ├── RegisterViewModel.cs
│           ├── LoginViewModel.cs
│           ├── ProductViewModel.cs
│           └── CheckoutViewModel.cs
├── Views/               # Razor views
│   ├── Home/
│   │   └── Index.cshtml           (✅ Product display)
│   ├── Regis/
│   │   ├── Register.cshtml        (⚠️ Template only)
│   │   └── SignIn.cshtml          (⚠️ Template only)
│   ├── CheckedOut/
│   │   └── Checked.cshtml         (⚠️ Basic)
│   ├── Shared/
│   │   └── _Layout.cshtml         (✅ Main layout)
│   └── [TO CREATE]:
│       ├── Account/
│       ├── Cart/
│       ├── Checkout/
│       └── Admin/
├── Services/            # ❌ TO CREATE
│   ├── AuthService.cs
│   ├── ProductService.cs
│   ├── CartService.cs
│   └── OrderService.cs
├── Filters/             # ❌ TO CREATE
│   └── CustomAuthorizeAttribute.cs
├── Scripts/             # JavaScript (jQuery, Bootstrap)
├── Web.config           # Configuration
└── packages.config      # NuGet dependencies
```

---

## 🗄️ Database Schema Summary

### Core Tables
1. **Users** - User accounts and roles
2. **Products** - Product catalog
3. **Categories** - Product categories
4. **Orders** - Customer orders
5. **OrderItems** - Order line items
6. **Carts** - Shopping carts (optional if using session)
7. **CartItems** - Cart line items
8. **Reviews** - Product reviews (bonus)

### User Roles
- **Admin**: Full access to dashboard and management
- **Customer**: Can shop, checkout, view orders
- **Guest**: Can browse, must register to checkout

---

## 🔐 Security Checklist

### Implemented
- ✅ Client-side validation (jQuery)
- ✅ Responsive design

### To Implement
- [ ] **SQL Injection**: Entity Framework (parameterized queries)
- [ ] **XSS**: @Html encoding in Razor views
- [ ] **CSRF**: @Html.AntiForgeryToken() in forms
- [ ] **Password Hashing**: BCrypt
- [ ] **Session Security**: httpOnly, secure cookies
- [ ] **Authorization**: Role-based access control
- [ ] **Input Sanitization**: Server-side validation

---

## 🎯 Feature Implementation Priority

### Phase 1: Foundation (CRITICAL)
1. MySQL database setup
2. Entity Framework configuration
3. Create all models
4. Connection string configuration

### Phase 2: Authentication (CRITICAL)
1. User registration
2. User login
3. Password hashing
4. Session management
5. Role-based authorization

### Phase 3: Core Features (HIGH)
1. Product CRUD (admin)
2. Product browsing (customer)
3. Shopping cart (backend)
4. Checkout process
5. Order management

### Phase 4: Admin Dashboard (HIGH)
1. Dashboard with statistics
2. Order management
3. Customer management
4. Sales reports

### Phase 5: Polish (MEDIUM)
1. Error handling
2. Validation messages
3. UI improvements
4. Testing

### Phase 6: Bonus (OPTIONAL)
1. Product reviews
2. Email notifications
3. Advanced analytics
4. Inventory alerts

---

## 📝 Key ViewModels to Create

### RegisterViewModel
```csharp
public class RegisterViewModel
{
    [Required]
    [StringLength(50, MinimumLength = 3)]
    public string Username { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 6)]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Compare("Password")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }
}
```

### LoginViewModel
```csharp
public class LoginViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    public bool RememberMe { get; set; }
}
```

---

## 🔧 Common Issues & Solutions

### Issue: MySQL Connection Failed
**Solution**: 
- Verify MySQL server is running
- Check connection string in Web.config
- Ensure MySQL port is 3306
- Verify username/password

### Issue: Entity Framework Not Found
**Solution**: 
```powershell
Install-Package EntityFramework -Version 6.4.4
```

### Issue: BCrypt Not Found
**Solution**:
```powershell
Install-Package BCrypt.Net-Next -Version 4.0.3
```

### Issue: Session Data Lost
**Solution**: 
- Check Web.config session timeout
- Ensure InProc session mode
- Verify cookies are enabled

---

## 🧪 Testing Checklist

### User Registration & Login
- [ ] Can register new customer account
- [ ] Password is hashed in database
- [ ] Can login with correct credentials
- [ ] Cannot login with wrong password
- [ ] Session persists across pages
- [ ] Can logout successfully

### Product Management (Admin)
- [ ] Admin can create new product
- [ ] Admin can edit existing product
- [ ] Admin can delete product (soft delete)
- [ ] Admin can view all products
- [ ] Validation works on product forms

### Shopping Cart
- [ ] Can add product to cart
- [ ] Can update quantity
- [ ] Can remove item
- [ ] Cart total calculates correctly
- [ ] Cart persists in session

### Checkout
- [ ] Cannot check out as guest
- [ ] Can place order as customer
- [ ] Order saves to database
- [ ] Stock is updated
- [ ] Cart is cleared after checkout
- [ ] Order confirmation shows

### Admin Dashboard
- [ ] Shows total orders
- [ ] Shows total revenue
- [ ] Shows recent orders
- [ ] Shows low stock products
- [ ] Can update order status
- [ ] Can view customer list
- [ ] Can deactivate customer

### Security
- [ ] Cannot access admin pages as customer
- [ ] Cannot access admin pages as guest
- [ ] CSRF tokens present in forms
- [ ] Passwords are hashed
- [ ] Input is validated server-side

---

## 📚 Important Code Snippets

### Hash Password
```csharp
string hash = BCrypt.Net.BCrypt.HashPassword(password);
```

### Verify Password
```csharp
bool isValid = BCrypt.Net.BCrypt.Verify(password, passwordHash);
```

### Check User Role
```csharp
if (Session["UserRole"]?.ToString() == "Admin")
{
    // Admin-only code
}
```

### Authorize Attribute
```csharp
[CustomAuthorize(RequiredRole = "Admin")]
public class AdminController : Controller
{
    // Controller code
}
```

### Session Cart
```csharp
var cart = Session["Cart"] as List<CartItemDTO>;
if (cart == null)
{
    cart = new List<CartItemDTO>();
    Session["Cart"] = cart;
}
```

---

## 🎓 Learning Resources

### Official Documentation
- [ASP.NET MVC](https://docs.microsoft.com/en-us/aspnet/mvc/)
- [Entity Framework 6](https://docs.microsoft.com/en-us/ef/ef6/)
- [MySQL Connector/NET](https://dev.mysql.com/doc/connector-net/en/)

### Tutorials
- [EF Code First with MySQL](https://dev.mysql.com/doc/connector-net/en/connector-net-entityframework60.html)
- [ASP.NET MVC Security](https://docs.microsoft.com/en-us/aspnet/mvc/overview/security/)
- [BCrypt Password Hashing](https://github.com/BcryptNet/bcrypt.net)

### Tools
- **Visual Studio**: Main IDE
- **MySQL Workbench**: Database management
- **Postman**: API testing (if adding API endpoints)
- **Browser DevTools**: Frontend debugging

---

## 💡 Pro Tips

1. **Start Simple**: Implement one feature at a time and test thoroughly
2. **Use Migrations**: Enable EF migrations for database schema changes
3. **Separation of Concerns**: Keep business logic in services, not controllers
4. **ViewModels**: Never pass entity models directly to views
5. **Validation**: Always validate both client and server-side
6. **Error Handling**: Use try-catch blocks and display friendly error messages
7. **Logging**: Add logging for debugging (NLog or log4net)
8. **Comments**: Comment complex business logic
9. **Git**: Commit frequently with descriptive messages
10. **Backup**: Regular database backups during development

---

## 📊 Progress Tracking

Use this checklist to track your implementation progress:

### Database Setup
- [ ] MySQL installed and running
- [ ] Database created
- [ ] Schema created (all tables)
- [ ] Initial data seeded
- [ ] Connection string configured

### Models
- [ ] ApplicationDbContext created
- [ ] User entity created
- [ ] Product entity created
- [ ] Order entity created
- [ ] OrderItem entity created
- [ ] Category entity created
- [ ] Cart entities created
- [ ] Review entity created (bonus)
- [ ] All ViewModels created

### Services
- [ ] AuthService created
- [ ] ProductService created
- [ ] CartService created
- [ ] OrderService created

### Controllers
- [ ] AccountController created
- [ ] ProductController created
- [ ] CartController created
- [ ] CheckoutController created
- [ ] AdminController created
- [ ] AdminProductController created

### Views
- [ ] Login view created
- [ ] Register view created
- [ ] Product list view updated
- [ ] Cart view created
- [ ] Checkout view created
- [ ] Order confirmation view created
- [ ] Admin dashboard view created
- [ ] Admin product management views created
- [ ] Admin order management views created

### Security
- [ ] CSRF tokens added to forms
- [ ] CustomAuthorize attribute created
- [ ] Password hashing implemented
- [ ] Session security configured
- [ ] Input validation implemented

### Testing
- [ ] Registration tested
- [ ] Login tested
- [ ] Product CRUD tested
- [ ] Shopping cart tested
- [ ] Checkout tested
- [ ] Admin functions tested
- [ ] Security tested

---

## 🆘 Need Help?

### Common Questions

**Q: Should I use ASP.NET Identity?**  
A: No, the requirement is to build custom authentication for learning purposes.

**Q: Session vs. Database for cart?**  
A: Use session for speed, optionally persist to database for registered users.

**Q: How to handle image uploads?**  
A: For now, use URLs. Implement file upload in bonus phase.

**Q: What about email verification?**  
A: Optional bonus feature. Start with basic registration first.

**Q: Should I use repository pattern?**  
A: Optional. Services are sufficient for this project size.

---

## 📅 Suggested 4-Week Timeline

### Week 1: Foundation
- Days 1-2: Database setup and EF configuration
- Days 3-4: Create all models
- Days 5-7: Authentication system

### Week 2: Core Features
- Days 8-10: Product management
- Days 11-12: Shopping cart
- Days 13-14: Checkout process

### Week 3: Admin Features
- Days 15-17: Admin dashboard
- Days 18-19: Order management
- Days 20-21: Customer management

### Week 4: Polish & Testing
- Days 22-23: Security hardening
- Days 24-25: Testing
- Days 26-28: Bug fixes and improvements

---

**Last Updated**: November 23, 2025  
**Version**: 1.0
