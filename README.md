# CleanArch - Employee Management System with GPS Location Tracking

A clean architecture-based ASP.NET Core 9.0 web API for comprehensive employee management with advanced GPS-based attendance tracking, vacation management, and department organization.

## 📋 Application Description

**CleanArch** is a robust backend system designed for managing employees, departments, attendance with GPS validation, and vacation requests. The application follows Clean Architecture principles with clear separation of concerns across different layers.

### 🎯 Key Features

- **User Authentication & Authorization** - JWT-based authentication with role management
- **Employee Management** - Complete CRUD operations for employee profiles
- **Department Management** - Hierarchical department structure with user assignment
- **📍 GPS-Based Attendance Tracking** - Check-in/check-out system with location validation
- **Vacation Management** - Vacation request submission, approval, and tracking
- **Multi-language Support** - Arabic and English localization
- **File Upload** - Profile image management with local storage
- **📍 Real-time Location Validation** - Ensure employees are within company premises

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

2. **Update Connection String and Company Location**
   - Open `CleanArch.Api/appsettings.json`
   - Modify the connection string and company location settings:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=CleanArchDb;Trusted_Connection=true;MultipleActiveResultSets=true"
   },
   "CompanyLocation": {
     "Latitude": 30.0444,
     "Longitude": 31.2357,
     "AllowedRadiusInMeters": 500,
     "CompanyName": "Your Company Name",
     "Address": "Company Address Here"
   }
   ```

3. **Recreate Database Migration**
   ```bash
   # Delete existing migration files from CleanArch.Infra/Migrations/ folder
   # Then create new migration with location tracking
   dotnet ef migrations add "AddLocationTrackingToAttendance" --project CleanArch.Infra --startup-project CleanArch.Api
   
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
├── CleanArch.App/          # Application layer (Features, Services, Location Validation)
├── CleanArch.Domain/       # Domain layer (Entities with GPS fields, Interfaces)
├── CleanArch.Infra/        # Infrastructure layer (Data, Repositories)
└── CleanArch.Common/       # Shared components (DTOs, Enums, Location Models)
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
- **📍 GPS Location Services** - Haversine formula for distance calculation

## 🗄️ Database Schema

The system includes entities for:
- **Users** (ApplicationUser with Identity)
- **Departments** (Hierarchical structure)
- **📍 AttendanceRecords** (Check-in/check-out tracking with GPS coordinates)
- **Vacations** (Leave management)
- **LeaveRequests** (Absence tracking)

## 🔐 Authentication & Authorization

The API uses JWT tokens with role-based access control. Supported roles include:
- Administrator
- Manager  
- Employee

## 📚 API Endpoints

### 🔐 Authentication
- `POST /api/auth/login` - User authentication
- `POST /api/auth/register` - User registration

### 👥 User Management
- `GET /api/users` - User management
- `PUT /api/users/profile` - Update user profile

### 🏢 Department Operations
- `GET /api/departments` - Department operations
- `POST /api/departments` - Create department

### 📍 Attendance Tracking (New GPS Features)
- `POST /api/attendance/checkin` - Check-in with GPS location validation
- `POST /api/attendance/checkout` - Check-out with location tracking
- `GET /api/attendance/today` - Get today's attendance record
- `GET /api/attendance/history` - Attendance history with location data

### 🏖️ Vacation Management
- `POST /api/vacations` - Submit vacation request
- `GET /api/vacations` - Get user vacations
- `PUT /api/vacations/{id}/approve` - Approve/reject vacation (Admin/Manager)

### 📋 Leave Management
- `POST /api/attendance/leave-request` - Submit leave request
- `GET /api/attendance/pending-leaves` - Get pending leaves (Admin/Manager)

## 🌐 Localization

The application supports both English (en-US) and Arabic (ar-EG) languages through resource files located in `CleanArch.Api/Resources/`.

## 📍 GPS Location Features

### Location Validation
- **Real-time GPS tracking** during check-in/check-out
- **Configurable radius** (default: 500 meters)
- **Haversine formula** for accurate distance calculation
- **Flexible location settings** per company branch

### Location Storage
- **Check-in/check-out coordinates** stored for auditing
- **Device information** and **IP address** tracking
- **Location history** for compliance and reporting

### Configuration
```json
"CompanyLocation": {
  "Latitude": 30.0444,
  "Longitude": 31.2357,
  "AllowedRadiusInMeters": 500,
  "CompanyName": "Your Company",
  "Address": "Company Address"
}
```

## 📧 Email Services

Configure email settings in `appsettings.json` under the `EmailSettings` section for password reset and notification functionality.

## 🗂️ File Storage

Profile images are stored locally in `wwwroot/Uploads/profile-images/` with configurable options in `UploadOptions`.

## 🧪 Testing

### Swagger Testing Examples

#### Check-in with Location:
```json
{
  "checkInTime": "2024-01-15T08:30:00",
  "latitude": 30.0444,
  "longitude": 31.2357,
  "deviceInfo": "iPhone 13, iOS 16.0",
  "ipAddress": "192.168.1.100"
}
```

#### Check-out with Location:
```json
{
  "checkOutTime": "2024-01-15T17:30:00",
  "latitude": 30.0444,
  "longitude": 31.2357,
  "deviceInfo": "iPhone 13, iOS 16.0", 
  "ipAddress": "192.168.1.100"
}
```

Use the provided `CleanArch.Api.http` file for testing API endpoints with REST Client extension.

## 🔄 Next Steps

After successful setup, you can:
1. Register a new admin user
2. Configure company location coordinates
3. Create departments and assign users
4. Test GPS-based attendance workflows
5. Configure location radius and policies
6. Generate location-based attendance reports

## 📞 Support

For issues during setup, check:
- Database connection string configuration
- .NET 9.0 SDK installation
- SQL Server service status
- Migration execution logs
- Company location coordinates accuracy
- GPS permissions for mobile applications

## 🆕 What's New in Location Features

### ✅ Added Features
- **GPS-based attendance validation**
- **Real-time location tracking**
- **Configurable company geofence**
- **Location audit trails**
- **Device and IP address tracking**
- **Enhanced security against remote abuse**

### 🎯 Business Benefits
- **Ensure physical presence** at workplace
- **Prevent attendance fraud**
- **Accurate location-based reporting**
- **Flexible location policies** per branch
- **Compliance with attendance regulations**

The system now provides enterprise-grade attendance tracking with advanced GPS location validation! 🎉
