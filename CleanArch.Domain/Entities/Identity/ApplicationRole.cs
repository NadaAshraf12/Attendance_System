using CleanArch.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CleanArch.Infra.Identity
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public ApplicationRole() : base() { }
        public ApplicationRole(string roleName) : base(roleName) { }

        // Optional: map to RoleType enum for seeding/consistency
        public RoleType RoleType { get; set; } = RoleType.User;

        [MaxLength(250)]
        public string? Description { get; set; }
    }

}
