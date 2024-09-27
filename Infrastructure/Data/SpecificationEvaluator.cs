using System.Linq;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class SpecificationEvaluator<T> where T : class
    {
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> spec)
        {
            // Start with the input query
            var query = inputQuery;

            // Apply filtering criteria
            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);
            }

            // Apply ordering
            if (spec.OrderBy != null)
            {
                query = query.OrderBy(spec.OrderBy);
            }
            else if (spec.OrderByDescending != null)
            {
                query = query.OrderByDescending(spec.OrderByDescending);
            }

            // Apply paging
            if (spec.IsPagingEnabled)
            {
                query = query.Skip(spec.Skip).Take(spec.Take);
            }

            // Apply includes (expression-based)
            query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));

            // Apply includes (string-based for nested properties)
            query = spec.IncludeStrings.Aggregate(query, (current, include) => current.Include(include));

            // Use AsNoTracking for read-only queries to improve performance
            query = query.AsNoTracking();

            return query;
        }
    }
}
