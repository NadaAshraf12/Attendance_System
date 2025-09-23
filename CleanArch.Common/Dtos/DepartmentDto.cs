﻿namespace CleanArch.Common.Dtos
{
    public class DepartmentDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;

        public List<DepartmentDto> SubDepartments { get; set; } = new();
    }
}
