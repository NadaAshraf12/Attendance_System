using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace CleanArch.Domain.Specifications
{
    public abstract class BaseSpecification<T> : ISpecification<T>
    {
        public Expression<Func<T, bool>> Criteria { get; }
        public List<Func<IQueryable<T>, IIncludableQueryable<T, object>>> Includes { get; } = new();
        public Expression<Func<T, object>> OrderBy { get; private set; }
        public Expression<Func<T, object>> OrderByDescending { get; private set; }
        public int? Skip { get; private set; }
        public int? Take { get; private set; }
        public bool IsPagingEnabled { get; private set; }
        public bool AsNoTracking { get; private set; } = true;
        public bool AsSplitQuery { get; private set; }

        protected BaseSpecification(Expression<Func<T, bool>> criteria) => Criteria = criteria;

        protected void AddInclude(Func<IQueryable<T>, IIncludableQueryable<T, object>> include) => Includes.Add(include);
        protected void AddOrderBy(Expression<Func<T, object>> orderBy) => OrderBy = orderBy;
        protected void AddOrderByDescending(Expression<Func<T, object>> orderByDesc) => OrderByDescending = orderByDesc;
        protected void ApplyPaging(int skip, int take) { Skip = skip; Take = take; IsPagingEnabled = true; }
        protected void DisableTracking() => AsNoTracking = false; 
        protected void EnableSplitQuery() => AsSplitQuery = true;
    }


}
