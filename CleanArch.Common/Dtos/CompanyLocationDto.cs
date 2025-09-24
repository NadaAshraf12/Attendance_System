using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArch.Common.Dtos
{
    public class CompanyLocationDto : LocationDto
    {
        public double AllowedRadiusInMeters { get; set; } = 500; // 500 متر افتراضي
    }
}
