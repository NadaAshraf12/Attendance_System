namespace CleanArch.Common.Dtos
{
    public class VacationDto
    {
        public Guid Id { get; set; }
        public string UserFullName { get; set; }
        public string DepartmentName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Days { get; set; }
        public string Status { get; set; }
    }

}
