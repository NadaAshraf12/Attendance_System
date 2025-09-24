using CleanArch.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArch.App.Interface
{
    public interface ILocationService
    {
        bool IsWithinRadius(LocationDto point1, LocationDto point2, double radiusInMeters);
        double CalculateDistance(LocationDto point1, LocationDto point2);
    }
}
