using CleanArch.App.Services;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CleanArch.App.Features.Users.Commands.UpdateProfile
{
    public record UpdateProfileCommand(
        string? UserName,
        string? PhoneNumber,
        string? FirstName,
        string? LastName,
        string? FullName,
        IFormFile? ProfilePicture,
        //Guid? CityId,
        string? Street,
        string? BuildingNumber,
        string? PostalCode,
        double? Latitude,
        double? Longitude,
        string? AdditionalNumber
    ) : IRequest<ResponseModel>;
}
