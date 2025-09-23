using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArch.Common.Dtos
{
    public class UserDto
    {
        public Guid Id { get; set; } = default!;
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public string? PhoneNumber { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool IsDeleted { get; set; }
        public IEnumerable<string> Roles { get; set; } = Enumerable.Empty<string>();

        public DepartmentDto? Department { get; set; } 

    }
}
