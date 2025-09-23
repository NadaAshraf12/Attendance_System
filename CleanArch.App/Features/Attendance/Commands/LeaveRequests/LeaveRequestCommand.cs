using CleanArch.App.Services;
using CleanArch.Common.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArch.App.Features.Attendance.Commands.LeaveRequests
{
    public class LeaveRequestCommand : IRequest<ResponseModel>
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public LeaveType LeaveType { get; set; }
        public string Reason { get; set; } = string.Empty;
    }
}
