using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

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
