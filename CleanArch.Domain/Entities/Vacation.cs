using CleanArch.Common.Enums;
using CleanArch.Domain.Base;
using CleanArch.Domain.Base.BaseInterface;
using CleanArch.Infra.Identity;
using System.ComponentModel.DataAnnotations;

namespace CleanArch.Domain.Entities
{
    public class Vacation : BaseEntity, ISoftDelete
    {
        public Guid UserId { get; set; }
        public ApplicationUser? User { get; set; }

        public VacationType Type { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // stored value
        public int Days { get; set; }

        public Guid? SubstituteId { get; set; }
        public ApplicationUser? Substitute { get; set; }

        public string? Reason { get; set; }

        public string? AttachmentPath { get; set; }

        public VacationStatus Status { get; set; } = VacationStatus.Pending;

        public Guid? ApprovedById { get; set; } // manager/admin who approved
        public DateTime? ApprovedAt { get; set; }

        [Timestamp]
        public byte[]? RowVersion { get; set; }
    }
}
