using System.Linq.Expressions;
using Unalive_WebManagement.DTOs;

namespace Unalive_WebManagement.BLL.Helpers
{
    public static class QueryHelper
    {
        public static IEnumerable<T> ApplyQuery<T>(this IEnumerable<T> source, QueryParameters query, Func<T, string?, bool> searchPredicate)
        {
            if (source == null) return Enumerable.Empty<T>();

            // Search
            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                source = source.Where(x => searchPredicate(x, query.Search));
            }

            // Sort
            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                var propertyInfo = typeof(T).GetProperty(query.SortBy, System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                if (propertyInfo != null)
                {
                    source = query.IsDescending 
                        ? source.OrderByDescending(x => propertyInfo.GetValue(x, null)) 
                        : source.OrderBy(x => propertyInfo.GetValue(x, null));
                }
            }

            return source;
        }
    }
}
