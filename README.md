# 🏢 Employee Portal System (EPS)

A comprehensive, production-ready employee management system built with **ASP.NET Core 8**, featuring Clean Architecture, JWT authentication, and role-based access control.

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)
![C#](https://img.shields.io/badge/C%23-12.0-239120?logo=csharp)
![SQL Server](https://img.shields.io/badge/SQL%20Server-LocalDB-CC2927?logo=microsoftsqlserver)
![Bootstrap](https://img.shields.io/badge/Bootstrap-5.3-7952B3?logo=bootstrap)
![License](https://img.shields.io/badge/license-MIT-green)



## ✨ Features

### 👥 Employee Management
- ✅ Complete CRUD operations with search, filter, and pagination
- ✅ Auto-generated employee IDs (EMP-XXXX format)
- ✅ Email uniqueness validation
- ✅ Manager-subordinate relationships
- ✅ Department and designation assignment

### 📅 Leave Management
- ✅ 7 leave types (Sick, Casual, Annual, Unpaid, etc.)
- ✅ Multi-level approval workflow
- ✅ Overlapping leave detection
- ✅ Leave balance tracking
- ✅ Real-time status updates

### ⏰ Attendance Tracking
- ✅ Daily check-in/check-out recording
- ✅ Attendance status (Present, Absent, Late, Half-Day)
- ✅ Date range filtering
- ✅ Working hours calculation
- ✅ Monthly attendance reports

### 🔐 Security & Authentication
- ✅ JWT Bearer authentication for API
- ✅ Cookie-based authentication for MVC
- ✅ Refresh token mechanism (7-day validity)
- ✅ Role-based access control (4 roles)
- ✅ Account lockout after 3 failed attempts

### 🎨 Modern UI
- ✅ Responsive Bootstrap 5 design
- ✅ Gradient-based color scheme
- ✅ Dashboard with statistics cards
- ✅ Role-based navigation
- ✅ Client-side validation

## 🏗️ Architecture

This project follows **Clean Architecture** principles with clear separation of concerns:

```
┌─────────────────────────────────────────┐
│           Presentation Layer            │
│     (API Controllers + MVC Views)       │
├─────────────────────────────────────────┤
│          Application Layer              │
│    (Services, DTOs, Interfaces)         │
├─────────────────────────────────────────┤
│        Infrastructure Layer             │
│  (EF Core, Repositories, Identity)      │
├─────────────────────────────────────────┤
│            Domain Layer                 │
│    (Entities, Enums, Business Rules)    │
└─────────────────────────────────────────┘
```

### Design Patterns
- **Repository Pattern**: Generic and specific repositories for data access
- **Unit of Work**: Transaction management
- **Dependency Injection**: Loose coupling
- **DTO Pattern**: Data transfer objects with AutoMapper
- **Service Layer Pattern**: Business logic separation

## 🛠️ Tech Stack

### Backend
- **ASP.NET Core 8.0** - Web framework
- **C# 12** - Programming language
- **Entity Framework Core 8.0** - ORM
- **ASP.NET Core Identity** - User management
- **SQL Server LocalDB** - Database
- **AutoMapper** - Object mapping
- **FluentValidation** - Input validation

### Authentication
- **JWT Bearer Tokens** - API authentication
- **Refresh Tokens** - Token renewal
- **Cookie Authentication** - MVC authentication

### Frontend
- **Razor Pages/MVC** - Server-side rendering
- **Bootstrap 5.3** - UI framework
- **jQuery** - AJAX calls
- **Bootstrap Icons** - Icon library

### API Documentation
- **Swagger/OpenAPI** - Interactive API documentation

## 🚀 Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server LocalDB](https://docs.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb) or SQL Server
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/EmployeePortalSystem.git
   cd EmployeePortalSystem
   ```

2. **Update connection string** (if needed)
   
   In `EPS.API/appsettings.json` and `EPS.Web/appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=EmployeePortalDB;Trusted_Connection=true"
   }
   ```

3. **Restore packages**
   ```bash
   dotnet restore
   ```

4. **Create database and run migrations**
   ```bash
   cd EPS.API
   dotnet ef database update
   ```

5. **Run the API**
   ```bash
   cd EPS.API
   dotnet run
   ```
   API will be available at: `https://localhost:5001`

6. **Run the Web Portal** (in a new terminal)
   ```bash
   cd EPS.Web
   dotnet run
   ```
   Web portal will be available at: `https://localhost:5002`

### Default Credentials (Debug Mode Only)

| Role | Email | Password |
|------|-------|----------|
| Admin | admin@eps.com | Admin@123 |
| HR Manager | hr@eps.com | HrManager@123 |

> ⚠️ **Note**: These credentials are only available in DEBUG mode and are automatically created during database seeding.

## 📁 Project Structure

```
EmployeePortalSystem/
│
├── EPS.Domain/                    # Core business entities
│   ├── Entities/                  # Domain models
│   └── Enums/                     # Enumerations
│
├── EPS.Infrastructure/            # Data access layer
│   ├── Data/                      # DbContext & Seeding
│   └── Repositories/              # Repository implementations
│
├── EPS.Application/               # Business logic layer
│   ├── DTOs/                      # Data transfer objects
│   ├── Interfaces/                # Service contracts
│   ├── Services/                  # Service implementations
│   └── Mappings/                  # AutoMapper profiles
│
├── EPS.API/                       # RESTful API
│   ├── Controllers/               # API endpoints
│   └── Middleware/                # Custom middleware
│
└── EPS.Web/                       # MVC Admin Portal
    ├── Controllers/               # MVC controllers
    └── Views/                     # Razor views
```


### Main API Endpoints

#### Authentication
- `POST /api/auth/login` - Login with credentials
- `POST /api/auth/register` - Register new user
- `POST /api/auth/refresh-token` - Refresh access token
- `POST /api/auth/logout` - Revoke refresh token

#### Employees
- `GET /api/employees` - Get all employees (paginated)
- `GET /api/employees/{id}` - Get employee by ID
- `POST /api/employees` - Create new employee
- `PUT /api/employees/{id}` - Update employee
- `DELETE /api/employees/{id}` - Delete employee

#### Leaves
- `GET /api/leaves/my-leaves` - Get current user's leaves
- `POST /api/leaves/request` - Request new leave
- `GET /api/leaves/pending` - Get pending approvals
- `PUT /api/leaves/approve/{id}` - Approve/reject leave

#### Attendance
- `POST /api/attendance/mark` - Mark attendance
- `GET /api/attendance/my-attendance` - Get own attendance
- `GET /api/attendance/employee/{id}` - Get employee attendance

## 🎯 Role-Based Access Control

| Role | Permissions |
|------|-------------|
| **Admin** | Full system access, user management, delete operations |
| **HR Manager** | Employee CRUD, leave approvals, reports, department management |
| **Manager** | View team employees, approve team leaves, mark attendance |
| **Employee** | View own profile, request leaves, view own attendance |

## 🗄️ Database Schema

### Key Tables
- **Employees** - Employee records with personal and employment details
- **Departments** - Organizational departments
- **Designations** - Job titles and roles
- **Leaves** - Leave requests with approval workflow
- **Attendance** - Daily attendance records
- **Documents** - Employee document attachments
- **AspNetUsers** - Identity users (linked to employees)
- **AspNetRoles** - System roles


---

