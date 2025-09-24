namespace CleanArch.Common.Dtos
{
    public class CompanyLocationDto : LocationDto
    {
        public double AllowedRadiusInMeters { get; set; } = 500; 
    }
}
