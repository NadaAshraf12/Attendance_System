using CleanArch.App.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArch.App.Features.Attendance.Commands.CheckIn
{
    public class CheckInCommand : IRequest<ResponseModel>
    {
        public DateTime CheckInTime { get; set; } = DateTime.Now;
    }
}
