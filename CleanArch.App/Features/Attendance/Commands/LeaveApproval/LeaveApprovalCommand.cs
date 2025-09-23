using CleanArch.App.Services;
using CleanArch.Common.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArch.App.Features.Attendance.Commands.LeaveApproval
{
    public class LeaveApprovalCommand : IRequest<ResponseModel>
    {
        public Guid LeaveRequestId { get; set; } // ID الطلب
        public LeaveStatus Status { get; set; } // Approved أو Rejected
    }
}
