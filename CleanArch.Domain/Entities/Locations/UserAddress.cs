using CleanArch.Domain.Base;
using System.ComponentModel.DataAnnotations;

namespace CleanArch.Domain.Entities.Locations
{
    public class UserAddress : BaseEntity<Guid>
    {
        [Required]
        public Guid UserId { get; set; }

        [Required(ErrorMessage =""), MaxLength(200, ErrorMessage ="")]
        public string Line1 { get; set; } = default!;

        [MaxLength(200)]
        public string? Line2 { get; set; }

        [MaxLength(150)]
        public string? District { get; set; }

        [MaxLength(150)]
        public string? CityName { get; set; }

        public Guid? CityId { get; set; }
        public virtual City? City { get; set; }

        [MaxLength(20)]
        public string? PostalCode { get; set; }

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public bool IsDefault { get; set; }

        
    }
}
