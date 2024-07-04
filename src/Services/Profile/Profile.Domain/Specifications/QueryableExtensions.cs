using Shared.Interfaces;

namespace Profile.Domain.Specifications;

public static class QueryableExtensions
{
    public static IQueryable<T> WhereNotDeleted<T>(this IQueryable<T> query) where T : class
    {
        if (typeof(ISoftDeletable).IsAssignableFrom(typeof(T)))
        {
            query = query.Where(e => ((ISoftDeletable)e).DeletedAt == null);
        }

        return query;
    }
}
