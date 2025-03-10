using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace datntdev.Abp.EntityFrameworkCore.Extensions;

/// <summary>
/// Extension methods for <see cref="IQueryable"/> and <see cref="IQueryable{T}"/>.
/// </summary>
public static class QueryableExtensions
{
    /// <summary>
    /// Specifies the related objects to include in the query results.
    /// </summary>
    /// <param name="source">The source <see cref="IQueryable{T}"/> on which to call Include.</param>
    /// <param name="condition">A boolean value to determine to include <paramref name="path"/> or not.</param>
    /// <param name="path">The dot-separated list of related objects to return in the query results.</param>
    public static IQueryable<T> IncludeIf<T>(this IQueryable<T> source, bool condition, string path)
        where T : class
    {
        return condition
            ? source.Include(path)
            : source;
    }

    /// <summary>
    /// Specifies the related objects to include in the query results.
    /// </summary>
    /// <param name="source">The source <see cref="IQueryable{T}"/> on which to call Include.</param>
    /// <param name="condition">A boolean value to determine to include <paramref name="path"/> or not.</param>
    /// <param name="path">The type of navigation property being included.</param>
    public static IQueryable<T> IncludeIf<T, TProperty>(this IQueryable<T> source, bool condition, Expression<Func<T, TProperty>> path)
        where T : class
    {
        return condition
            ? source.Include(path)
            : source;
    }

    /// <summary>
    /// Specifies the related objects to include in the query results.
    /// </summary>
    /// <param name="source">The source <see cref="IQueryable{T}"/> on which to call Include.</param>
    /// <param name="condition">A boolean value to determine to include <paramref name="include"/> or not.</param>
    /// <param name="include">A function to include one or more navigation properties using Include/ThenInclude chaining operators.</param>
    public static IQueryable<T> IncludeIf<T>(
        this IQueryable<T> source,
        bool condition,
        Func<IQueryable<T>, IIncludableQueryable<T, object>> include)
        where T : class
    {
        return condition
            ? include(source)
            : source;
    }
}