using CleanArch.Domain.Base;
using System.ComponentModel.DataAnnotations;

namespace CleanArch.Domain.Entities.Locations
{
    public class City : BaseEntity<Guid>
    {
        [Required, MaxLength(150)]
        public string NameAr { get; set; } = default!;

        [Required, MaxLength(150)]
        public string NameEn { get; set; } = default!;

        [MaxLength(10)]
        public string? CountryCode { get; set; }

        [MaxLength(10)]
        public string? RegionCode { get; set; }

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public virtual ICollection<UserAddress> UserAddresses { get; set; } = new List<UserAddress>();
    }
}
