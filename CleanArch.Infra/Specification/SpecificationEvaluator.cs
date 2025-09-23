using CleanArch.Domain.Base;
using Microsoft.EntityFrameworkCore;


namespace CleanArch.Domain.Specifications
{
    public static class SpecificationEvaluator<T> where T : BaseEntity<Guid>
    {
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> specification)
        {
            var query = inputQuery;

            if (specification.AsNoTracking) query = query.AsNoTracking();
            if (specification.AsSplitQuery) query = query.AsSplitQuery();

            if (specification.Criteria is not null)
                query = query.Where(specification.Criteria);

            if (specification.OrderBy is not null)
                query = query.OrderBy(specification.OrderBy);
            else if (specification.OrderByDescending is not null)
                query = query.OrderByDescending(specification.OrderByDescending);

            // Includes with ThenInclude support
            query = specification.Includes.Aggregate(query, (current, include) => include(current));

            if (specification.IsPagingEnabled)
            {
                if (specification.Skip is int s) query = query.Skip(s);
                if (specification.Take is int t) query = query.Take(t);
            }

            return query;
        }

    }
}
