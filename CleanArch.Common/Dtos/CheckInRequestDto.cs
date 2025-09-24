using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArch.Common.Dtos
{
    public class CheckInRequestDto
    {
        public DateTime? CheckInTime { get; set; }

        [Range(-90, 90, ErrorMessage = "Latitude must be between -90 and 90")]
        public double? Latitude { get; set; }

        [Range(-180, 180, ErrorMessage = "Longitude must be between -180 and 180")]
        public double? Longitude { get; set; }

        public string? DeviceInfo { get; set; }
        public string? IpAddress { get; set; }
    }
}
