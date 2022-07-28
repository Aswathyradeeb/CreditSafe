using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EventsApp.Framework
{
    /// <summary>
    /// Represents an abstraction of the unit of work pattern. 
    /// </summary>
    /// <remarks>This is a high-level abstraction that delegates the actual unit of work implementation to the used ORM</remarks>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// save the current changes to the underline Data store
        /// </summary>
        void Save();

        /// <summary>
        /// save the current changes to the underline data store in async
        /// </summary>
        /// <returns></returns>
        Task<int> SaveAsync();


        /// <summary>
        /// save the current changes to the underline data store in async and provide the ability to cancel the save process
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> SaveAsync(CancellationToken cancellationToken);


        void Dispose(bool disposing);

        /// <summary>
        /// Commits the changes in the current unit of work
        /// </summary>
        void Commit();

        /// <summary>
        /// Rollback the changes in the current unit of work
        /// </summary>
        void Rollback();



        /// <summary>
        /// Rollback the changes in the current unit of work in async mode
        /// </summary>
        Task<int> CommitAsync();


        /// <summary>
        /// Returns <code>IQueryable</code> object that allows querying the given entity's data source.
        /// </summary>
        /// <typeparam name="TEntity">Type of the entity to return its queryable data source</typeparam>
        /// <returns><code>IQueryable</code> object of the given entity</returns>
        IQueryable<TEntity> EntitySet<TEntity>() where TEntity : class;

        /// <summary>
        /// Return the underlying ORM Session/Context
        /// </summary>
        /// <typeparam name="TContext">Expected type of the underlying ORM session/context</typeparam>
        /// <returns>Return the underlying ORM Context</returns>
        TContext OrmContext<TContext>() where TContext : class;

    }
}
