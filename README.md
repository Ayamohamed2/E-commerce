# E-Commerce Project

This is a full-stack E-Commerce web application built with **ASP.NET Core MVC** and **Entity Framework Core**, implementing a complete shopping experience with user management, product catalog, orders, and feedback system.

---

## Features

### **User Management**
- User registration and login.
- **External login** using **Google** and **Facebook**.
- Users can update their profile information including **email** and **password**.
- **Forgot Password** functionality.
- Role-based access:
  - Admin
  - Employee
  - Company
  - Customer

### **Admin Panel**
- **Category Management**
  - Create, update, delete categories.
- **Company Management**
  - Manage companies, view all companies, delete and edit.
- **Product Management**
  - Add, edit, delete products.
  - Upload multiple images per product.
- **Order Management**
  - View all orders and details.
  - Update order status, track shipments.
  - Cancel orders and process refunds via **Stripe**.
- **User Management**
  - Assign roles to users.
  - Lock/Unlock user accounts.

### **Customer Features**
- Browse products with category and image support.
- Product details page with feedback.
- Add to **Shopping Cart** and manage quantities.
- **Checkout** with Stripe payment integration.
- View order summary and order confirmation.
- Submit feedback and view product ratings.

### **Feedback System**
- Users can post, edit, and delete feedback for products.
- Shows average rating for each product.
- Users can see feedback along with profile pictures.

---

## Technologies & Tools

- **Backend:** ASP.NET Core MVC, C#, Entity Framework Core
- **Frontend:** Razor Pages, HTML5, CSS3, Bootstrap
- **Authentication:** ASP.NET Identity, External Login (Google & Facebook)
- **Payments:** Stripe Payment Gateway
- **Database:** SQL Server
- **Email:** ASP.NET Core Identity Email Sender
- **Version Control:** Git & GitHub

---
