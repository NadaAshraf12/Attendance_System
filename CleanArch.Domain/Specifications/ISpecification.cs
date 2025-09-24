using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace CleanArch.Domain.Specifications
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> Criteria { get; }
        List<Func<IQueryable<T>, IIncludableQueryable<T, object>>> Includes { get; }
        Expression<Func<T, object>> OrderBy { get; }
        Expression<Func<T, object>> OrderByDescending { get; }
        int? Skip { get; }
        int? Take { get; }
        bool IsPagingEnabled { get; }
        bool AsNoTracking { get; }
        bool AsSplitQuery { get; }
    }

}
