using CleanArch.Domain.Entities;
using CleanArch.Domain.Repositories;
using CleanArch.Infra.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArch.Infra.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ApplicationDbContext _context;
        public DepartmentRepository(ApplicationDbContext context) => _context = context;

        public async Task<Department> AddAsync(Department department)
        {
            _context.Departments.Add(department);
            await _context.SaveChangesAsync();
            return department;
        }

        public Task<Department?> GetByIdAsync(Guid id) =>
            _context.Departments.Include(d => d.Users).FirstOrDefaultAsync(d => d.Id == id);

        public Task<List<Department>> GetAllAsync() =>
            _context.Departments.ToListAsync();

        public Task<Department?> GetByNameAsync(string name) =>
            _context.Departments.FirstOrDefaultAsync(d => d.Name == name);

        public async Task<Department> UpdateAsync(Department department)
        {
            department.UpdatedAt = DateTime.UtcNow;
            department.UpdatedOn = DateTime.UtcNow;

            _context.Departments.Update(department);
            await _context.SaveChangesAsync();
            return department;
        }

        /// <summary>
        /// Soft-delete: mark IsDeleted = true, set timestamps, and detach users from this department (set their DepartmentId = null)
        /// Returns true if deleted, false if not found.
        /// </summary>
        public async Task<bool> SoftDeleteAsync(Guid id)
        {
            var dept = await _context.Departments
                .Include(d => d.Users)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (dept == null) return false;
            if (dept.IsDeleted) return false;

            // mark soft deleted
            dept.IsDeleted = true;
            dept.IsActive = false;
            dept.DeletedAt = DateTime.UtcNow;
            dept.DeletedOn = DateTime.UtcNow;

            // optional: detach users from department (set to null)
            if (dept.Users != null && dept.Users.Any())
            {
                foreach (var user in dept.Users)
                {
                    user.DepartmentId = null;
                    // If ApplicationUser tracked under same context, change will be saved.
                    _context.Users.Update(user);
                }
            }

            _context.Departments.Update(dept);
            await _context.SaveChangesAsync();
            return true;
        }

        public Task<Department?> GetByCodeAsync(string code) =>
            _context.Departments.FirstOrDefaultAsync(d => d.Code == code);

        public async Task<IEnumerable<Department>> GetByIdsAsync(IEnumerable<Guid> ids)
        {
            return await _context.Departments
                .Where(d => ids.Contains(d.Id) && !d.IsDeleted)
                .ToListAsync();
        }

        public async Task<Department?> GetByIdWithChildrenAsync(Guid id)
        {
            return await _context.Departments
                .Include(d => d.SubDepartments) // 👈 نجيب الـ SubDepartments
                .FirstOrDefaultAsync(d => d.Id == id && !d.IsDeleted);
        }

        public async Task<Department?> GetByCodeWithChildrenAsync(string code)
        {
            return await _context.Departments
                .Include(d => d.SubDepartments)
                .FirstOrDefaultAsync(d => d.Code == code && !d.IsDeleted);
        }

        public async Task<List<Department>> GetAllWithChildrenAsync()
        {
            return await _context.Departments
                .Include(d => d.SubDepartments) 
                .Where(d => d.ParentDepartmentId == null) 
                .ToListAsync();
        }


    }

}
