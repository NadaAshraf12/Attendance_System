namespace CleanArch.App.Features.Users.Commands.UpdateProfile
{
    public class UserProfileDto
    {
        public string Id { get; set; } = string.Empty;
        public string? UserName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FullName { get; set; }
        public string? ProfilePicture { get; set; }
        //public int? CityId { get; set; }
        public string? Street { get; set; }
        public string? BuildingNumber { get; set; }
        public string? PostalCode { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string? AdditionalNumber { get; set; }
    }
}
