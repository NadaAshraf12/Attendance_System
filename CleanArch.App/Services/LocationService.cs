using CleanArch.App.Interface;
using CleanArch.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArch.App.Services
{
    public class LocationService : ILocationService
    {
        public bool IsWithinRadius(LocationDto point1, LocationDto point2, double radiusInMeters)
        {
            var distance = CalculateDistance(point1, point2);
            return distance <= radiusInMeters;
        }

        public double CalculateDistance(LocationDto point1, LocationDto point2)
        {
            // Haversine formula لحساب المسافة بين نقطتين على الكرة الأرضية
            var earthRadius = 6371000; // نصف قطر الأرض بالمتر

            var lat1Rad = DegreesToRadians(point1.Latitude);
            var lat2Rad = DegreesToRadians(point2.Latitude);
            var deltaLat = DegreesToRadians(point2.Latitude - point1.Latitude);
            var deltaLon = DegreesToRadians(point2.Longitude - point1.Longitude);

            var a = Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) +
                    Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                    Math.Sin(deltaLon / 2) * Math.Sin(deltaLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return earthRadius * c;
        }

        private double DegreesToRadians(double degrees)
        {
            return degrees * (Math.PI / 180);
        }
    }
}
