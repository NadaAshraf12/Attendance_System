using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArch.Common.Dtos
{
    public record SubDepartmentDto(
        Guid? Id,          // null معناها جديد
        string Name,
        string Code,
        string? Description,
        bool IsActive
    );
}
