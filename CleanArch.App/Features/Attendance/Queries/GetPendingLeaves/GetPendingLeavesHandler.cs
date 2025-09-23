using CleanArch.Domain.Entities;
using CleanArch.Domain.Repositories.Command;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArch.App.Features.Attendance.Queries.GetPendingLeaves
{
    public class GetPendingLeavesHandler : IRequestHandler<GetPendingLeavesQuery, List<LeaveRequest>>
    {
        private readonly ILeaveRepository _leaveRepository;

        public GetPendingLeavesHandler(ILeaveRepository leaveRepository)
        {
            _leaveRepository = leaveRepository;
        }

        public async Task<List<LeaveRequest>> Handle(GetPendingLeavesQuery request, CancellationToken cancellationToken)
        {
            return await _leaveRepository.GetPendingLeavesAsync();
        }
    }
}
