# 🛒 FreshMarket E-Commerce

A full-featured e-commerce web application built with ASP.NET MVC Framework and MySQL database. This project demonstrates practical implementation of MVC architecture, secure user authentication, and complete shopping cart functionality.

![ASP.NET MVC](https://img.shields.io/badge/ASP.NET-MVC%205-blue)
![MySQL](https://img.shields.io/badge/MySQL-8.0-orange)
![License](https://img.shields.io/badge/License-MIT-green)

---

## 📋 Table of Contents

- [Features](#-features)
- [Tech Stack](#-tech-stack)
- [Installation](#-installation)
- [Database Setup](#-database-setup)
- [Running the Application](#-running-the-application)
- [User Manual](#-user-manual)
- [Sample Credentials](#-sample-credentials)
- [Project Structure](#-project-structure)
- [Security Features](#-security-features)

---

## ✨ Features

### For Administrators
- 📊 **Dashboard** - Overview of sales, orders, and customer statistics
- 📦 **Product Management** - Add, edit, delete, and view products with image upload
- 🛍️ **Order Management** - View and update order status
- 👥 **Customer Management** - View customer details, activate/deactivate accounts
- 📈 **Sales Reports** - View order summaries and revenue reports

### For Customers
- 🔐 **User Authentication** - Secure registration and login with BCrypt password hashing
- 🛒 **Shopping Cart** - Add products, update quantities, remove items
- 💳 **Checkout** - Complete purchase with shipping information
- 📋 **Order History** - Track past orders and status

### For Guests
- 👁️ **Browse Products** - View product catalog without an account
- 🔍 **Search & Filter** - Find products by category or search term
- 📝 **Register Prompt** - Prompted to sign up when attempting checkout

---

## 🛠️ Tech Stack

| Component | Technology |
|-----------|------------|
| **Framework** | ASP.NET MVC 5 (.NET Framework 4.8) |
| **Database** | MySQL 8.0 |
| **ORM** | Dapper (Micro-ORM) |
| **Authentication** | Session-based with BCrypt password hashing |
| **Frontend** | HTML5, CSS3, JavaScript |
| **Icons** | Phosphor Icons |
| **Animations** | AOS (Animate On Scroll) |

---

## 📥 Installation

### Prerequisites

1. **Visual Studio 2019/2022** with ASP.NET and web development workload
2. **MySQL Server 8.0+** (or XAMPP/WAMP with MySQL)
3. **MySQL Workbench** (optional, for database management)
4. **.NET Framework 4.8** Runtime

### Step 1: Clone the Repository

```bash
git clone https://github.com/yourusername/MVC-Ecommerce-IPT.git
cd MVC-Ecommerce-IPT
```

### Step 2: Restore NuGet Packages

Open the solution in Visual Studio and restore NuGet packages:

```
Right-click Solution → Restore NuGet Packages
```

Or via Package Manager Console:
```powershell
Update-Package -reinstall
```

---

## 🗄️ Database Setup

### Step 1: Create the Database

1. Open **MySQL Workbench** or your MySQL client
2. Run the schema file located at:
   ```
   ECommerce/Database/ecommerce_schema.sql
   ```

This will create:
- `ecommerce_db` database
- All required tables (users, products, orders, cart_items, etc.)
- Sample data (products, categories)

### Step 2: Configure Connection String

Open `ECommerce/Web.config` and update the connection string:

```xml
<connectionStrings>
  <add name="DefaultConnection" 
       connectionString="Server=localhost;Database=ecommerce_db;Uid=root;Pwd=YOUR_PASSWORD;" 
       providerName="MySql.Data.MySqlClient" />
</connectionStrings>
```

Replace `YOUR_PASSWORD` with your MySQL root password.

### Step 3: Add Sample Users (Optional)

Run the sample users script for quick testing:
```
ECommerce/Database/sample_users_hashed.sql
```

---

## 🚀 Running the Application

### Option 1: Visual Studio (Recommended)

1. Open `ECommerce.sln` in Visual Studio
2. Set `ECommerce` as the startup project
3. Press **F5** or click **Start Debugging**
4. The browser will open to `http://localhost:xxxx`

### Option 2: IIS Express

1. Right-click the project → **Properties**
2. Go to **Web** tab
3. Select **IIS Express** and configure the port
4. Press **Ctrl+F5** to run without debugging

---

## 📖 User Manual

### Guest User

1. **Browse Products**
   - Visit the homepage to see featured products
   - Use the category cards to filter by type (Leafy, Root, Fruit, Herbs)
   - Click on any product to view details

2. **Add to Cart**
   - Click the cart icon on any product
   - Products are stored in your browser until you sign in

3. **Checkout**
   - Click "Proceed to Checkout" in the cart
   - You will be prompted to sign in or create an account

### Customer User

1. **Register an Account**
   - Click "Sign Up" in the navigation
   - Fill in your details (First Name, Last Name, Email, Password)
   - Your cart items will be preserved after registration

2. **Sign In**
   - Click "Sign In" in the navigation
   - Enter your email and password

3. **Shopping**
   - Browse and add products to cart
   - View your cart by clicking the cart icon
   - Update quantities or remove items as needed

4. **Checkout Process**
   - Click "Proceed to Checkout"
   - Enter shipping information
   - Select payment method (simulation only)
   - Click "Place Order" to complete

5. **View Orders**
   - Go to "My Account" → "My Orders"
   - Track order status (Pending, Processing, Shipped, Delivered)

### Admin User

1. **Access Admin Panel**
   - Sign in with admin credentials
   - You will be redirected to the Admin Dashboard

2. **Dashboard**
   - View total revenue, orders, and customer counts
   - See recent orders at a glance

3. **Manage Products**
   - Navigate to "Products" in the sidebar
   - **Add Product**: Click "Add Product" button, fill form, upload image
   - **Edit Product**: Click the edit icon on any product row
   - **Delete Product**: Click the delete icon (with confirmation)

4. **Manage Orders**
   - Navigate to "Orders" in the sidebar
   - View all orders with customer details
   - Change order status using the dropdown

5. **Manage Customers**
   - Navigate to "Customers" in the sidebar
   - View customer list with order history
   - Toggle active/inactive status
   - View customer details

6. **View Reports**
   - Navigate to "Reports" in the sidebar
   - See sales statistics and order summaries

---

## 🔑 Sample Credentials

### Admin Account
| Field | Value |
|-------|-------|
| Email | `admin@freshmarket.com` |
| Password | `admin123` |

### Customer Accounts
| Name | Email | Password |
|------|-------|----------|
| Justin Rivera | `justinrivera@gmail.com` | `justinrivera` |


> **Note:** For new accounts registered through the website, passwords are automatically hashed using BCrypt.

---

## 📁 Project Structure

```
MVC-Ecommerce-IPT/
├── ECommerce/
│   ├── Controllers/
│   │   ├── Api/                    # API Controllers
│   │   │   ├── AuthApiController.cs
│   │   │   ├── CartApiController.cs
│   │   │   ├── OrdersApiController.cs
│   │   │   ├── ProductsApiController.cs
│   │   │   └── UsersApiController.cs
│   │   ├── AdminController.cs
│   │   ├── CartController.cs
│   │   ├── CustomerController.cs
│   │   ├── HomeController.cs
│   │   └── RegisController.cs
│   │
│   ├── Models/
│   │   ├── User.cs
│   │   ├── Product.cs
│   │   ├── Order.cs
│   │   ├── OrderItem.cs
│   │   ├── CartItem.cs
│   │   ├── Category.cs
│   │   └── ViewModels/
│   │
│   ├── Repositories/               # Data Access Layer
│   │   ├── UserRepository.cs
│   │   ├── ProductRepository.cs
│   │   ├── OrderRepository.cs
│   │   └── CartRepository.cs
│   │
│   ├── Views/
│   │   ├── Admin/                  # Admin Views
│   │   │   ├── Index.cshtml
│   │   │   ├── Products.cshtml
│   │   │   ├── Orders.cshtml
│   │   │   ├── Customers.cshtml
│   │   │   └── Reports.cshtml
│   │   ├── Cart/                   # Shopping Cart
│   │   ├── Customer/               # Customer Dashboard
│   │   ├── Home/                   # Public Pages
│   │   ├── Regis/                  # Authentication
│   │   └── Shared/                 # Layout & Partials
│   │
│   ├── Content/                    # Static Assets
│   │   ├── css/
│   │   └── images/
│   │
│   ├── Database/
│   │   ├── ecommerce_schema.sql    # Database Schema
│   │   └── sample_users_hashed.sql # Sample Users
│   │
│   └── Web.config                  # Configuration
│
└── README.md
```

---

## 🔒 Security Features

| Feature | Implementation |
|---------|----------------|
| **Password Hashing** | BCrypt.Net with salt |
| **SQL Injection Prevention** | Dapper parameterized queries |
| **Session Management** | ASP.NET Session state |
| **Authentication Required** | Checkout requires login |
| **Input Validation** | Client and server-side validation |
| **XSS Prevention** | Razor HTML encoding |

---

## 📝 License

This project is created for educational purposes as part of IPT (Integrative Programming and Technologies) coursework.

---

## 👨‍💻 Authors

- **@jayjayandcattos** 

---

## 🙏 Acknowledgments

- Phosphor Icons for beautiful iconography
- AOS Library for scroll animations
- Unsplash for product images
