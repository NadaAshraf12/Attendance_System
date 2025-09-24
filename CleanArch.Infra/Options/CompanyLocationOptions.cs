using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArch.Infra.Options
{
    public class CompanyLocationOptions
    {
        public const string SectionName = "CompanyLocation";

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double AllowedRadiusInMeters { get; set; } = 500;
        public string CompanyName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }
}
