using CleanArch.Domain.Base.BaseInterface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        // Optional optimistic concurrency
        [Timestamp]
        public byte[]? RowVersion { get; set; }
    }

    // إذا كنت تحتاج BaseEntity بدون generic parameter
    public abstract class BaseEntity : BaseEntity<Guid>
    {
    }


}

