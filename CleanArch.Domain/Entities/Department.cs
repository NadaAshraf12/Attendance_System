using CleanArch.Domain.Base;
using CleanArch.Domain.Base.BaseInterface;
using CleanArch.Infra.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArch.Domain.Entities
{
    // CleanArch.Domain/Entities/Department.cs
    public class Department : BaseEntity, ISoftDelete, IHasTimestamps
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Code { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        // Users
        public virtual ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
        public virtual ApplicationUser? Manager { get; set; }
        public Guid? ManagerId { get; set; }

        // ✅ Sub-departments
        public Guid? ParentDepartmentId { get; set; }
        public virtual Department? ParentDepartment { get; set; }
        public virtual ICollection<Department> SubDepartments { get; set; } = new List<Department>();

        // Soft delete
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }

        // Timestamps
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

}
