namespace CleanArch.Common.Dtos
{
    public class CheckInOutDto : LocationDto
    {
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public string? DeviceInfo { get; set; }
        public string? IpAddress { get; set; }
    }
}
