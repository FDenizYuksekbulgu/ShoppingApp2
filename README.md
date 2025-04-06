# 🛒 **ShoppingApp2 - Online Shopping Platform**

## 📜 **Project Overview**

**ShoppingApp2** is an **ASP.NET Core Web API** project that provides the core API services for an online shopping platform. This project uses a **multi-layered architecture** to separate database interactions, business logic, and API services. It manages database operations with **Entity Framework Core** and implements user authentication and authorization using **JWT (JSON Web Token)**. Users can browse products, place orders, and interact with a shopping platform managed by an admin.

## 🛠 **Project Requirements**

1. **Layered Architecture**:
   - **Presentation Layer** (API Layer): Contains the controllers that handle API requests.
   - **Business Layer**: Contains the business logic and application rules.
   - **Data Access Layer**: Handles database interactions via Entity Framework, using the Repository and UnitOfWork design patterns.

2. **Data Models**:
   - **User**: Stores customer information.
   - **Product**: Stores products available for sale.
   - **Order**: Stores customer orders.
   - **OrderProduct**: Manages the many-to-many relationship between orders and products.

3. **Authentication and Authorization**:
   - User authentication is performed using **ASP.NET Core Identity** or a custom identity service.
   - **JWT** is used for token-based authentication and role-based authorization.

4. **Middleware**:
   - **Logging Middleware**: Logs each incoming request, including the URL, time, and user information.
   - **Maintenance Middleware**: Handles incoming requests when the application is in maintenance mode.

5. **Action Filter**:
   - **Action Filter**: Provides access control for API endpoints, allowing access during specific time windows.

6. **Model Validation**:
   - Applies necessary validation rules for customer and product models (e.g., email format, required fields, stock quantity).

7. **Dependency Injection**:
   - Manages services using dependency injection.

8. **Data Protection**:
   - User passwords are securely stored using the **Data Protection** API.

9. **Exception Handling**:
   - Implements global exception handling, catching all errors and returning appropriate responses.

## 🖥 **Technologies Used**

- **ASP.NET Core**: Used for developing the Web API.
- **Entity Framework Core**: Interacts with the database using the Code First approach.
- **JWT (JSON Web Tokens)**: Used for user authentication and authorization.
- **ASP.NET Core Identity** or custom identity service: Used for user identity management.
- **Dependency Injection**: Manages service dependencies.
- **Middleware**: Logs each incoming request and handles maintenance mode.
- **Data Protection API**: Securely stores passwords.
- **Action Filters**: Controls access to API endpoints.

## 🏗 **Project Structure**

- **Presentation Layer**: Contains controller classes. API requests are handled here and routed to business logic.
- **Business Layer**: Contains classes that implement the business logic.
- **Data Access Layer**: Contains classes that handle database operations and repositories.

## 🚀 **Getting Started**

Before running the project in your development environment, follow the steps below:

1. **Download Project Files**:
   - Clone the project using Git:
     ```bash
     git clone <repository_url>
     ```

2. **Apply Database Migrations**:
   - Create and apply the necessary migrations for the database:
     ```bash
     dotnet ef migrations add InitialCreate
     dotnet ef database update
     ```

3. **Install Dependencies**:
   - After opening the project, install the necessary NuGet packages:
     ```bash
     dotnet restore
     ```

4. **Start the Application**:
   - Run the application using the following command:
     ```bash
     dotnet run
     ```

5. **Test the API**:
   - Once the application is running, you can test the API using Postman or any HTTP client.

## 📡 **API Usage**

There are various endpoints available in the API that allow users to view products, place orders, and administrators to manage the platform.

Example API Endpoints:

- **GET /api/products**: Lists all products.
- **POST /api/orders**: Creates a new order.
- **POST /api/auth/login**: User login and JWT token generation.
- **POST /api/auth/register**: Register a new user.

## 🔄 **Communication Between Layers**

- **Presentation Layer** (API Layer) calls business logic through the **Business Layer**.
- The **Business Layer** interacts with the **Data Access Layer** for data operations.

## 👨‍💻 **Developer Information**

This project is a multi-layered web API developed using **ASP.NET Core**. The goal of the project is to provide essential functionalities for an online shopping platform while maintaining a scalable solution built with a clean software architecture.

The technology stack used at the start of the project is chosen in accordance with modern web application development standards.
