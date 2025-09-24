using CleanArch.Common.Const;
using CleanArch.Common.Enums;
using CleanArch.Domain.Base;
using CleanArch.Domain.Entities;
using CleanArch.Domain.Entities.Locations;
using CleanArch.Domain.Enums;
using CleanArch.Infra.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection.Emit;

namespace CleanArch.Infra.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public DbSet<Vacation> Vacations { get; set; }

        public DbSet<City> Cities { get; set; }
        public DbSet<UserAddress> UserAddresses { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        // ✅ Add DbSets for your entities
        public DbSet<AttendanceRecord> AttendanceRecords { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }

        public DbSet<Department> Departments { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions)
            : base(dbContextOptions) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Identity tables naming
            builder.Entity<ApplicationUser>(entity => entity.ToTable("Users"));
            builder.Entity<ApplicationRole>(entity => entity.ToTable("Roles"));
            builder.Entity<IdentityUserRole<Guid>>(entity => entity.ToTable("UserRoles"));
            builder.Entity<IdentityUserClaim<Guid>>(entity => entity.ToTable("UserClaims"));
            builder.Entity<IdentityUserLogin<Guid>>(entity => entity.ToTable("UserLogins"));
            builder.Entity<IdentityRoleClaim<Guid>>(entity => entity.ToTable("RoleClaims"));
            builder.Entity<IdentityUserToken<Guid>>(entity => entity.ToTable("UserTokens"));

            // --- Seed Roles from Enum ---
            builder.Entity<ApplicationRole>().HasData(
                new ApplicationRole
                {
                    Id = RoleSeedIds.User,
                    Name = RoleType.User.ToString(),
                    NormalizedName = RoleType.User.ToString().ToUpper(),
                    RoleType = RoleType.User,
                    Description = "Regular user"
                },
                new ApplicationRole
                {
                    Id = RoleSeedIds.Admin,
                    Name = RoleType.Admin.ToString(),
                    NormalizedName = RoleType.Admin.ToString().ToUpper(),
                    RoleType = RoleType.Admin,
                    Description = "Administrator"
                },
                new ApplicationRole
                {
                    Id = RoleSeedIds.Manager,
                    Name = RoleType.Manager.ToString(),
                    NormalizedName = RoleType.Manager.ToString().ToUpper(),
                    RoleType = RoleType.Manager,
                    Description = "Manager"
                }
            );


            // Relationships
            builder.Entity<ApplicationUser>()
                .HasMany(u => u.UserAddresses)
                .WithOne()
                .HasForeignKey(ua => ua.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Config لـ Department
            builder.Entity<Department>(entity =>
            {
                entity.ToTable("Department");

                entity.HasIndex(d => d.Code).IsUnique();

                entity.Property(d => d.Name)
                      .HasMaxLength(200)
                      .IsRequired();

                // علاقة Department ↔ Users
                entity.HasMany(d => d.Users)
                      .WithOne(u => u.Department)
                      .HasForeignKey(u => u.DepartmentId)
                      .OnDelete(DeleteBehavior.SetNull);

                // ✅ علاقة Department ↔ SubDepartments (Self Referencing)
                entity.HasOne(d => d.ParentDepartment)
                      .WithMany(d => d.SubDepartments)
                      .HasForeignKey(d => d.ParentDepartmentId)
                      .OnDelete(DeleteBehavior.Restrict);
                // Restrict: عشان لما يتشال Parent ما يمسحش كل الـ SubDepartments بالغلط
            });


            builder.Entity<ApplicationUser>()
                .HasOne(u => u.Department)
                .WithMany(d => d.Users)
                .HasForeignKey(u => u.DepartmentId)
                .OnDelete(DeleteBehavior.SetNull);


            builder.Entity<UserAddress>()
                .HasOne(ua => ua.City)
                .WithMany(c => c.UserAddresses)
                .HasForeignKey(ua => ua.CityId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ApplicationUser>()
                .HasOne(u => u.City)
                .WithMany()
                .HasForeignKey(u => u.CityId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.Entity<ApplicationUser>().HasIndex(u => u.Email).IsUnique();
            builder.Entity<ApplicationUser>().HasIndex(u => u.PhoneNumber);
            builder.Entity<City>().HasIndex(c => c.CountryCode);
            builder.Entity<City>().HasIndex(c => c.RegionCode);
            builder.Entity<RefreshToken>()
                .HasIndex(r => new { r.UserId, r.Token })
                .IsUnique();

            // Global query filters (Soft Delete)
            builder.Entity<ApplicationUser>().HasQueryFilter(u => !u.IsDeleted);
            builder.Entity<UserAddress>().HasQueryFilter(ua => !ua.IsDeleted);
            builder.Entity<City>().HasQueryFilter(c => !c.IsDeleted);
            builder.Entity<RefreshToken>().HasQueryFilter(r => !r.IsDeleted);

            builder.Entity<Vacation>(b =>
            {
                b.ToTable("Vacation"); // ✅ الجدول اسمه Vacation
                b.Property(p => p.Days).IsRequired();
                b.Property(p => p.Status).HasDefaultValue(VacationStatus.Pending);
                b.HasOne(p => p.User).WithMany().HasForeignKey(p => p.UserId).OnDelete(DeleteBehavior.Restrict);
                b.HasOne(p => p.Substitute).WithMany().HasForeignKey(p => p.SubstituteId).OnDelete(DeleteBehavior.Restrict);
                b.Property(p => p.RowVersion).IsRowVersion();
            });

            // Apply configurations from assembly
            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }

        public override int SaveChanges()
        {
            HandleSoftDeleteAndTimestamps();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            HandleSoftDeleteAndTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void HandleSoftDeleteAndTimestamps()
        {
            var now = DateTime.UtcNow;

            foreach (var entry in ChangeTracker.Entries<BaseEntity<Guid>>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedOn = now;
                        break;

                    case EntityState.Modified:
                        entry.Entity.UpdatedOn = now;
                        break;

                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.Entity.IsDeleted = true;
                        entry.Entity.DeletedOn = now;
                        break;
                }
            }
        }

        protected ApplicationDbContext() : base()
        {
        }
    }
}
