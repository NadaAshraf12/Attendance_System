using CleanArch.App.Services;
using MediatR;

namespace CleanArch.App.Features.Attendance.Commands.CheckOut
{
    public class CheckOutCommand : IRequest<ResponseModel>
    {
        public DateTime CheckOutTime { get; set; } = DateTime.Now;
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string? DeviceInfo { get; set; }
        public string? IpAddress { get; set; }
    }
}