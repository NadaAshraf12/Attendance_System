using CleanArch.Domain.Base.BaseInterface;
using System.ComponentModel.DataAnnotations;

namespace CleanArch.Domain.Base
{
    public abstract class BaseEntity<TKey> : ISoftDelete, IHasTimestamps
    {
        [Key]
        public TKey Id { get; set; } = default!;

        [MaxLength(150)]
        public string? CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        [MaxLength(150)]
        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public bool IsDeleted { get; set; }

        [MaxLength(150)]
        public string? DeletedBy { get; set; }

        public DateTime? DeletedOn { get; set; }

        [Timestamp]
        public byte[]? RowVersion { get; set; }
    }

    public abstract class BaseEntity : BaseEntity<Guid>
    {
    }


}

