﻿using System.Linq;

namespace datntdev.Abp.ObjectMapping
{
    /// <summary>
    /// Defines a simple interface to map objects.
    /// </summary>
    public interface IObjectMapper
    {
        /// <summary>
        /// Converts an object to another. Creates a new object of <typeparamref name="TDestination"/>.
        /// </summary>
        /// <typeparam name="TDestination">Type of the destination object</typeparam>
        /// <param name="source">Source object</param>
        TDestination Map<TDestination>(object source);

        /// <summary>
        /// Execute a mapping from the source object to the existing destination object
        /// </summary>
        /// <typeparam name="TSource">Source type</typeparam>
        /// <typeparam name="TDestination">Destination type</typeparam>
        /// <param name="source">Source object</param>
        /// <param name="destination">Destination object</param>
        /// <returns>Returns the same <paramref name="destination"/> object after mapping operation</returns>
        TDestination Map<TSource, TDestination>(TSource source, TDestination destination);

        /// <summary>
        /// Project the input queryable.
        /// </summary>
        /// <remarks>Projections are only calculated once and cached</remarks>
        /// <typeparam name="TDestination">Destination type</typeparam>
        /// <param name="source">Queryable source</param>
        /// <returns>Queryable result, use queryable extension methods to project and execute result</returns>
        IQueryable<TDestination> ProjectTo<TDestination>(IQueryable source);
    }
}
