using CleanArch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArch.Domain.Repositories
{
    public interface IVacationRepository
    {
        Task<Vacation> AddAsync(Vacation vacation);
        Task<Vacation?> GetByIdAsync(Guid id);
        Task<Vacation?> GetByIdWithUserAsync(Guid id);
        Task<IEnumerable<Vacation>> GetPendingForManagerAsync(Guid managerId); 
        Task ApproveAsync(Vacation vacation, Guid approverId);
        Task DeclineAsync(Vacation vacation, Guid approverId, string? reason);
        Task<IEnumerable<Vacation>> GetByUserAsync(Guid userId);

        Task UpdateAsync(Vacation vacation);
        Task DeleteAsync(Vacation vacation);

        Task<bool> HasVacationOnDateAsync(Guid userId, DateTime date, CancellationToken cancellationToken);

    }

}
