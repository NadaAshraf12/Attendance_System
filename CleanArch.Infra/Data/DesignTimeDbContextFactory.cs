// CleanArch.Infra/Data/DesignTimeDbContextFactory.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace CleanArch.Infra.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            // ابحث عن appsettings.json في مشروع الـ API
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "CleanArch.Api");

            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.Development.json", optional: true)
                .Build();

            // Build DbContextOptions
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Could not find connection string named 'DefaultConnection'");
            }

            builder.UseSqlServer(connectionString, options =>
            {
                options.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
            });

            return new ApplicationDbContext(builder.Options);
        }
    }
}