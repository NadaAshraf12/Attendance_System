using CleanArch.Domain.Repositories;
using CleanArch.Domain.Repositories.Command;
using CleanArch.Domain.Repositories.Command.Base;
using CleanArch.Domain.Repositories.Query.Base;
using CleanArch.Infra.Data;
using CleanArch.Infra.Identity;
using CleanArch.Infra.Options;
using CleanArch.Infra.Repositories;
using CleanArch.Infra.Repositories.Command;
using CleanArch.Infra.Repositories.Command.Base;
using CleanArch.Infra.Repositories.Query.Base;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CleanArch.Infra
{
    public static class DependencyInjectionInfra
    {
        // Fix: Add a method to register dependencies, accepting IServiceCollection and IConfiguration.
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // DbContext
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // DbConnector
            services.AddScoped<DbConnector>();

            // Identity
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<IAttendanceRepository, AttendanceRepository>();
            services.AddScoped<ILeaveRepository, LeaveRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IVacationRepository, VacationRepository>();

            services.Configure<CompanyLocationOptions>(
                configuration.GetSection(CompanyLocationOptions.SectionName));


            // Configure Lockout options
            services.Configure<IdentityOptions>(options =>
            {
                options.Lockout.MaxFailedAccessAttempts = 2;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);

                options.Lockout.AllowedForNewUsers = true;
            });

            services.AddScoped(typeof(ICommandRepository<>), typeof(CommandRepository<>));
            services.AddScoped(typeof(IQueryRepository<>), typeof(QueryRepository<>));

            // JWT Authentication
            var jwtSection = configuration.GetSection("Jwt");
            var jwtSettings = new
            {
                Issuer = jwtSection["Issuer"],
                Audience = jwtSection["Audience"],
                SigningKey = jwtSection["SigningKey"]
            };

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SigningKey))
                };
            });

            return services;
        }

    }
}
