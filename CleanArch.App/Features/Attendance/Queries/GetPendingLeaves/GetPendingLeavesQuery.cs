using CleanArch.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArch.App.Features.Attendance.Queries.GetPendingLeaves
{
    public class GetPendingLeavesQuery : IRequest<List<LeaveRequest>>
    {
    }
}
