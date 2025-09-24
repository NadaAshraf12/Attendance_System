using CleanArch.Common.Dtos;

namespace CleanArch.App.Interface
{
    public interface ILocationService
    {
        bool IsWithinRadius(LocationDto point1, LocationDto point2, double radiusInMeters);
        double CalculateDistance(LocationDto point1, LocationDto point2);
    }
}
