using CleanArch.Domain.Entities;
using CleanArch.Domain.Entities.Locations;
using CleanArch.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CleanArch.Infra.Identity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        [Required, MaxLength(150)]
        public string FirstName { get; set; } = default!;

        [Required, MaxLength(150)]
        public string LastName { get; set; } = default!;

        [Required, MaxLength(250)]
        public string FullName { get; set; } = default!;

        [MaxLength(1024)]
        public string? ProfilePicture { get; set; }

        [MaxLength(150)]
        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        [MaxLength(150)]
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        // Navigation to RefreshTokens (preferred separate table)
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

        [MaxLength(150)]
        public string? ActivationCode { get; set; }

        [MaxLength(150)]
        public string? ForgetPasswordCode { get; set; }

        [MaxLength(200)]
        public string? DeviceId { get; set; }

        public Guid? CityId { get; set; }
        public virtual City? City { get; set; }

        [MaxLength(200)]
        public string? Street { get; set; }

        [MaxLength(50)]
        public string? BuildingNumber { get; set; }

        [MaxLength(20)]
        public string? PostalCode { get; set; }

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        [MaxLength(50)]
        public string? AdditionalNumber { get; set; }

        public virtual ICollection<UserAddress> UserAddresses { get; set; } = new List<UserAddress>();

        // Legacy single refresh token fields (optional). Prefer the collection above.
        [MaxLength(500)]
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }

        // Soft delete for users
        public bool IsDeleted { get; set; }

        // Optional: quick role hint (Identity manages roles via junction table)
        public RoleType? RoleHint { get; set; }

        // إضافة DepartmentId
        public Guid? DepartmentId { get; set; }

        // Navigation property
        public virtual Department? Department { get; set; }
    }
}
