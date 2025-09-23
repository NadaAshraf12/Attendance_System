using CleanArch.Domain.Base;
using System.ComponentModel.DataAnnotations;

namespace CleanArch.Infra.Identity
{
    public class RefreshToken : BaseEntity<Guid>
    {
        [Required]
        public Guid UserId { get; set; }
        public virtual ApplicationUser User { get; set; } = default!;

        [Required, MaxLength(512)]
        public string Token { get; set; } = default!;

        [MaxLength(512)]
        public string? JwtId { get; set; } // link to JWT jti claim if needed

        public DateTime ExpiresOn { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? RevokedOn { get; set; }

        public bool IsRevoked => RevokedOn.HasValue;
        public bool IsExpired => DateTime.UtcNow >= ExpiresOn;
        public bool IsActive => !IsRevoked && !IsExpired;

        [MaxLength(50)]
        public string? Device { get; set; } // e.g., "android", "ios", "web"
        [MaxLength(200)]
        public string? DeviceId { get; set; }
        [MaxLength(45)]
        public string? IpAddress { get; set; }
    }
}
