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
    public class LeaveRequest : BaseEntity<Guid>
    {
        public Guid UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public LeaveType LeaveType { get; set; }
        public LeaveStatus Status { get; set; }
        public string Reason { get; set; }

        public ApplicationUser User { get; set; }
    }
}
