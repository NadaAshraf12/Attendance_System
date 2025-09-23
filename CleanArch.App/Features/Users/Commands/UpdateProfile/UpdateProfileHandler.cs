using CleanArch.App.Features.Users.Commands.UpdateProfile;
using CleanArch.App.Interface;
using CleanArch.App.Interface.Storage;
using CleanArch.App.Services;
using CleanArch.Infra.Identity;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;

public class UpdateProfileHandler : IRequestHandler<UpdateProfileCommand, ResponseModel>
{
    private readonly ICurrentUserService _current;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IResponseModel _response;
    private readonly IMapper _mapper;
    private readonly IFileStorage _fileStorage; // 🟢 هنستخدمه

    public UpdateProfileHandler(
        ICurrentUserService current,
        UserManager<ApplicationUser> userManager,
        IResponseModel response,
        IMapper mapper,
        IFileStorage fileStorage)
    {
        _current = current;
        _userManager = userManager;
        _response = response;
        _mapper = mapper;
        _fileStorage = fileStorage;
    }

    public async Task<ResponseModel> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var userId = _current.UserId ?? throw new UnauthorizedAccessException();
        var user = await _userManager.FindByIdAsync(userId.ToString())
                   ?? throw new KeyNotFoundException();

        bool IsValidString(string? value) =>
            !string.IsNullOrWhiteSpace(value) && value != "string";

        if (IsValidString(request.UserName))
            user.UserName = request.UserName!;
        if (IsValidString(request.PhoneNumber))
            user.PhoneNumber = request.PhoneNumber!;
        if (IsValidString(request.FirstName))
            user.FirstName = request.FirstName!;
        if (IsValidString(request.LastName))
            user.LastName = request.LastName!;
        if (IsValidString(request.FullName))
            user.FullName = request.FullName!;

        // 🟢 هنا هنرفع الصورة الجديدة ونحدث الرابط
        if (request.ProfilePicture is not null)
        {
            using var stream = request.ProfilePicture.OpenReadStream();

            var fileResource = await _fileStorage.SaveAsync(
                stream,
                request.ProfilePicture.FileName,       // اسم الملف الأصلي
                "Uploads/profile-images",              // الفولدر
                cancellationToken);                    // التوكن

            if (fileResource is not null)
            {
                user.ProfilePicture = fileResource.Url; // خزّن الرابط الناتج
            }
        }


        if (IsValidString(request.Street))
            user.Street = request.Street!;
        if (IsValidString(request.BuildingNumber))
            user.BuildingNumber = request.BuildingNumber!;
        if (IsValidString(request.PostalCode))
            user.PostalCode = request.PostalCode!;
        if (request.Latitude.HasValue && request.Latitude.Value != 0)
            user.Latitude = request.Latitude;
        if (request.Longitude.HasValue && request.Longitude.Value != 0)
            user.Longitude = request.Longitude;
        if (IsValidString(request.AdditionalNumber))
            user.AdditionalNumber = request.AdditionalNumber!;

        var res = await _userManager.UpdateAsync(user);
        if (!res.Succeeded)
            throw new InvalidOperationException(string.Join(" | ", res.Errors.Select(e => e.Description)));

        var dto = _mapper.Map<UserProfileDto>(user);

        return _response.Response(200, false, "Profile updated successfully.", dto);
    }
}
