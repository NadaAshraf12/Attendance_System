# CleanArch - Employee Management System

A clean architecture-based ASP.NET Core 9.0 web API for comprehensive employee management with attendance tracking, vacation management, and department organization.

## 📋 Application Description

**CleanArch** is a robust backend system designed for managing employees, departments, attendance, and vacation requests. The application follows Clean Architecture principles with clear separation of concerns across different layers.

### 🎯 Key Features

- **User Authentication & Authorization** - JWT-based authentication with role management
- **Employee Management** - Complete CRUD operations for employee profiles
- **Department Management** - Hierarchical department structure with user assignment
- **Attendance Tracking** - Check-in/check-out system with leave requests
- **Vacation Management** - Vacation request submission, approval, and tracking
- **Multi-language Support** - Arabic and English localization
- **File Upload** - Profile image management with local storage

### 🏗️ Architecture Layers

1. **Domain** - Core business entities and interfaces
2. **Application** - Business logic and use cases (CQRS pattern)
3. **Infrastructure** - Data access and external services
4. **API** - Presentation layer with controllers
5. **Common** - Shared DTOs, enums, and constants

## 🚀 Getting Started

### Prerequisites

- .NET 9.0 SDK
- SQL Server (LocalDB or Express)
- Git

### Installation & Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/NadaAshraf12/Attendance_System.git
   cd "New folder"
   ```

2. **Update Connection String**
   - Open `CleanArch.Api/appsettings.json`
   - Modify the `ConnectionStrings.DefaultConnection` to match your SQL Server instance:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=CleanArchDb;Trusted_Connection=true;MultipleActiveResultSets=true"
   }
   ```

3. **Recreate Database Migration**
   ```bash
   # Delete existing migration files from CleanArch.Infra/Migrations/ folder
   # Then create new migration
   dotnet ef migrations add "InitialCreate" --project CleanArch.Infra --startup-project CleanArch.Api
   
   # Update database
   dotnet ef database update --project CleanArch.Infra --startup-project CleanArch.Api
   ```

4. **Run the Application**
   ```bash
   dotnet run --project CleanArch.Api
   ```

   The API will be available at `https://localhost:7000` and `http://localhost:5000`

## 📁 Project Structure

```
CleanArch/
├── CleanArch.Api/          # Web API layer (Controllers, Program.cs)
├── CleanArch.App/          # Application layer (Features, Services)
├── CleanArch.Domain/       # Domain layer (Entities, Interfaces)
├── CleanArch.Infra/        # Infrastructure layer (Data, Repositories)
└── CleanArch.Common/       # Shared components (DTOs, Enums)
```

## 🔧 Key Technologies Used

- **ASP.NET Core 9.0** - Web framework
- **Entity Framework Core** - ORM and data access
- **MediatR** - CQRS pattern implementation
- **FluentValidation** - Validation framework
- **Mapster** - Object mapping
- **JWT Bearer Authentication** - Secure API access
- **Serilog** - Structured logging
- **Localization** - Multi-language support (Arabic/English)

## 🗄️ Database Schema

The system includes entities for:
- **Users** (ApplicationUser with Identity)
- **Departments** (Hierarchical structure)
- **AttendanceRecords** (Check-in/check-out tracking)
- **Vacations** (Leave management)
- **LeaveRequests** (Absence tracking)

## 🔐 Authentication & Authorization

The API uses JWT tokens with role-based access control. Supported roles include:
- Administrator
- Manager  
- Employee

## 📚 API Endpoints

- `POST /api/auth/login` - User authentication
- `POST /api/auth/register` - User registration
- `GET /api/users` - User management
- `GET /api/departments` - Department operations
- `POST /api/attendance/checkin` - Attendance tracking
- `GET /api/vacations` - Vacation management

## 🌐 Localization

The application supports both English (en-US) and Arabic (ar-EG) languages through resource files located in `CleanArch.Api/Resources/`.

## 📧 Email Services

Configure email settings in `appsettings.json` under the `EmailSettings` section for password reset and notification functionality.

## 🗂️ File Storage

Profile images are stored locally in `wwwroot/Uploads/profile-images/` with configurable options in `UploadOptions`.

## 🧪 Testing

Use the provided `CleanArch.Api.http` file for testing API endpoints with REST Client extension.

## 🔄 Next Steps

After successful setup, you can:
1. Register a new admin user
2. Create departments and assign users
3. Test attendance and vacation workflows
4. Configure additional settings as needed

## 📞 Support

For issues during setup, check:
- Database connection string configuration
- .NET 9.0 SDK installation
- SQL Server service status
- Migration execution logs
