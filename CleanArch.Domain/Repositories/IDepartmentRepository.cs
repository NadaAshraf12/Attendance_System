using CleanArch.Domain.Entities;

namespace CleanArch.Domain.Repositories
{
    public interface IDepartmentRepository
    {
        Task<Department> AddAsync(Department department);
        Task<Department?> GetByIdAsync(Guid id);
        Task<List<Department>> GetAllAsync();
        Task<Department?> GetByNameAsync(string name);

        Task<Department> UpdateAsync(Department department);
        Task<bool> SoftDeleteAsync(Guid id);
        Task<Department?> GetByCodeAsync(string code);
        Task<IEnumerable<Department>> GetByIdsAsync(IEnumerable<Guid> ids);
        Task<Department?> GetByIdWithChildrenAsync(Guid id);

        Task<Department?> GetByCodeWithChildrenAsync(string code);
        Task<List<Department>> GetAllWithChildrenAsync();


    }

}
