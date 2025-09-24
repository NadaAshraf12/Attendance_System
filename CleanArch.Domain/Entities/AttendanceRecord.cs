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

        //Gps Coordinates
        public double? CheckInLatitude { get; set; }
        public double? CheckInLongitude { get; set; }
        public double? CheckOutLatitude { get; set; }
        public double? CheckOutLongitude { get; set; }
        public string? CheckInDeviceInfo { get; set; }
        public string? CheckOutDeviceInfo { get; set; }
        public string? CheckInIpAddress { get; set; }
        public string? CheckOutIpAddress { get; set; }

        // Navigation property
        public ApplicationUser User { get; set; }
    }
}
