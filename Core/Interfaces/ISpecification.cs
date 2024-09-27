using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Core.Interfaces
{
    public interface ISpecification<T>
    {
        // Criteria for filtering
        Expression<Func<T, bool>> Criteria { get; }

        // Includes for eager loading
        List<Expression<Func<T, object>>> Includes { get; }
        // Include strings for nested navigation properties
        List<string> IncludeStrings { get; } 
        // Sorting
        Expression<Func<T, object>> OrderBy { get; }
        Expression<Func<T, object>> OrderByDescending { get; }

        // Pagination
        int Take { get; }
        int Skip { get; }
        bool IsPagingEnabled { get; }
    }
}
