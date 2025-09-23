using CleanArch.Common.Enums;
using CleanArch.Domain.Base;
using CleanArch.Domain.Base.BaseInterface;
using CleanArch.Infra.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArch.Domain.Entities
{
    public class AttendanceRecord : BaseEntity<Guid>
    {
        public Guid UserId { get; set; }
        public DateTime CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public AttendanceStatus Status { get; set; }
        public string? Notes { get; set; }

        // Navigation property
        public ApplicationUser User { get; set; }
    }
}
