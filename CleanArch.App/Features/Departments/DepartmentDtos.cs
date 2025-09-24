namespace CleanArch.App.Features.Departments
{
    public record DepartmentDto(Guid Id, string Name, string Code, bool IsActive);

    public class DepartmentUserDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    public class DepartmentDetailDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public Guid? ManagerId { get; set; }
        public bool IsDeleted { get; set; }

        public List<DepartmentUserDto> Users { get; set; } = new();
        public List<DepartmentDetailDto> SubDepartments { get; set; } = new(); 
    }
}
