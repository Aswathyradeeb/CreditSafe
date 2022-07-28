using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EventsApp.Framework
{
    /// <summary>
    /// Base Interface of the Repository pattern
    /// </summary>
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {

        /// <summary>
        /// insert an item into repository
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="item">Item to add to repository</param>
        void Insert(TEntity item);

        /// <summary>
        /// update an item into repository
        /// </summary>
        /// <param name="item">Item to update in repository</param>
        void Update(TEntity item);
        void Delete(TEntity item);



        /// <summary>
        /// Get all elements of type {T} in repository
        /// </summary>
        /// <returns>List of selected elements</returns>
        IQueryable<TEntity> GetAll();

        /// <summary>
        /// Get all elements of type {T} in repository in async 
        /// </summary>
        /// <returns>task with List of selected elements</returns>
        Task<IEnumerable<TEntity>> GetAllAsync();

        /// <summary>
        /// Returns number of items in the repository
        /// </summary>
        long Count { get; }

        /// <summary>
        /// Returns number of items in the repository in async
        /// </summary>
        Task<int> CountAsync { get; }


        /// <summary>
        /// Get  elements of type {T} in repository in Async
        /// </summary>
        /// <param name="filter">Filter that each element do match</param>
        /// <returns>List of selected elements</returns>
        Task<int> GetCountAsync(Expression<Func<TEntity, bool>> filter);

        /// <summary>
        /// Checks if the item exists in the repository
        /// </summary>
        /// <param name="item">Item to check if it exists in the repository or not</param>
        /// <returns>True if the item is found in the repository otherwise false</returns>
        bool Contains(TEntity item);

        /// <summary>
        /// Checks if the item exists in the repository in Async
        /// </summary>
        /// <param name="item">Item to check if it exists in the repository or not</param>
        /// <returns>True if the item is found in the repository otherwise false</returns>
        Task<bool> ContainsAsync(TEntity item);

        /// <summary>
        /// Get all elements of type {T} in repository
        /// </summary>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageCount">Number of elements in each page</param>
        /// <param name="orderByExpression">Order by expression for this query</param>
        /// <param name="ascending">Specify if order is ascending</param>
        /// <returns>List of selected elements</returns>
        IEnumerable<TEntity> Query<TOrderBy>(int pageIndex, int pageCount, Expression<Func<TEntity, TOrderBy>> orderByExpression, bool ascending);

        /// <summary>
        /// Get all elements of type {T} in repository in async
        /// </summary>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageCount">Number of elements in each page</param>
        /// <param name="orderByExpression">Order by expression for this query</param>
        /// <param name="ascending">Specify if order is ascending</param>
        /// <returns>List of selected elements</returns>
        Task<List<TEntity>> QueryAsync<TOrderBy>(int pageIndex, int pageCount, Expression<Func<TEntity, TOrderBy>> orderByExpression, bool @ascending);

        /// <summary>
        /// Get  elements of type {T} in repository
        /// </summary>
        /// <param name="filter">Filter that each element do match</param>
        /// <returns>List of selected elements</returns>
        IEnumerable<TEntity> Query(Expression<Func<TEntity, bool>> filter);

        /// <summary>
        /// Get  elements of type {T} in repository in Async
        /// </summary>
        /// <param name="filter">Filter that each element do match</param>
        /// <returns>List of selected elements</returns>
        Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> filter);

        /// <summary>
        /// Get all elements of type {T} in repository
        /// </summary>
        /// <param name="filter">Filter that each element do match</param>
        /// <param name="pageIndex">Index of page</param>
        /// <param name="pageCount">Number of elements in each page</param>
        /// <param name="orderByExpression">Order by expression</param>
        /// <param name="ascending">Specify if order is ascending</param>
        /// <returns>List of selected elements</returns>
        IEnumerable<TEntity> Query<TOrderBy>(Expression<Func<TEntity, bool>> filter, int pageIndex, int pageCount, Expression<Func<TEntity, TOrderBy>> orderByExpression, bool ascending);

        /// <summary>
        /// Get all elements of type {T} in repository in Async
        /// </summary>
        /// <param name="filter">Filter that each element do match</param>
        /// <param name="pageIndex">Index of page</param>
        /// <param name="pageCount">Number of elements in each page</param>
        /// <param name="orderByExpression">Order by expression</param>
        /// <param name="ascending">Specify if order is ascending</param>
        /// <returns>List of selected elements</returns>
        Task<IEnumerable<TEntity>> QueryAsync<TOrderBy>(Expression<Func<TEntity, bool>> filter, int pageIndex, int pageCount, Expression<Func<TEntity, TOrderBy>> orderByExpression, bool ascending);
        void Commit();
    }
}
