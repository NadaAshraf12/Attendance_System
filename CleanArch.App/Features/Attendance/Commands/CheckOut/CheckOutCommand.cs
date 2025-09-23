using CleanArch.App.Services;
using MediatR;

namespace CleanArch.App.Features.Attendance.Commands.CheckOut
{
    public class CheckOutCommand : IRequest<ResponseModel>
    {
        public DateTime CheckOutTime { get; set; } = DateTime.Now;
    }
}
