
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EventsApp.Framework
{
    public class RepositoryBase<TEntity, TKey> : IKeyedRepository<TEntity, TKey>
        where TEntity : class
        where TKey : IEquatable<TKey>
    {
        #region Fields

        protected IUnitOfWork UnitOfWork { get; set; }
        protected DbContext DbContext { get; set; }

        protected IQueryable<TEntity> DbSet { get; set; }

        protected DbSet<TEntity> SourceDbSet { get; set; }

        private bool _disposed;

        #endregion

        #region properties
        #endregion
        #region Constructor and Finalizers
        public RepositoryBase(IUnitOfWork UnitOfWork)
        {
            if (UnitOfWork == null)
                throw new ArgumentNullException("UnitOfWork");
            this.UnitOfWork = UnitOfWork;

            DbContext = UnitOfWork.OrmContext<DbContext>();
            SourceDbSet = DbContext.Set<TEntity>();
            DbSet = SourceDbSet;
        }
        #endregion

        #region Implementation of IRepository<TEntity>

        /// <summary>
        /// Save an item into repository
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="item">Item to add to repository</param>
        public void Insert(TEntity item)
        {

            DbEntityEntry dbEntityEntry = DbContext.Entry(item);
            if (dbEntityEntry.State != EntityState.Detached)
            {
                dbEntityEntry.State = EntityState.Added;
            }
            else
            {
                // add entity to the operation context
                SourceDbSet.Add(item);

            }
        }

        /// <summary>
        /// update an item into repository
        /// </summary>
        /// <param name="item">Item to update in repository</param>
        public void Update(TEntity item)
        {
            if (DbContext.Entry(item).State == EntityState.Detached)
            {
                SourceDbSet.Attach(item);
            }
            DbContext.Entry(item).State = EntityState.Modified;
        }


        /// <summary>
        /// delete an item into repository
        /// </summary>
        /// <param name="item">Item to Delete in repository</param>
        public void Delete(TEntity item)
        {
            if (DbContext.Entry(item).State == EntityState.Detached)
            {
                SourceDbSet.Attach(item);
            }
            DbContext.Entry(item).State = EntityState.Deleted;
        }


        /// <summary>
        /// Get all elements of type {T} in repository
        /// </summary>
        /// <returns>List of selected elements</returns>
        public IQueryable<TEntity> GetAll()
        {

            return DbSet;
        }

        public void Commit()
        {
            DbContext.SaveChanges(); 
        }
        /// <summary>
        /// Get all elements of type {T} in repository async
        /// </summary>
        /// <returns>List of selected elements</returns>
        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            var list = await DbSet.ToListAsync();
            return list.ToList();
        }

        /// <summary>
        /// Returns number of items in the repository
        /// </summary>
        public long Count
        {
            get
            {
                return this.DbSet.Count();
            }
        }

        /// <summary>
        /// Returns number of items in the repository async
        /// </summary>
        public Task<int> CountAsync
        {
            get
            {
                return this.DbSet.CountAsync();
            }
        }

        /// <summary>
        /// Returns number of items in the repository async
        /// </summary>
        public async Task<int> GetCountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await this.DbSet.CountAsync(predicate);
        }

        /// <summary>
        /// Checks if the item exists in the repository
        /// </summary>
        /// <param name="item">Item to check if it exists in the repository or not</param>
        /// <returns>True if the item is found in the repository otherwise false</returns>
        public bool Contains(TEntity item)
        {
            return DbSet.Contains(item);
        }

        /// <summary>
        /// Checks if the item exists in the repository async
        /// </summary>
        /// <param name="item">Item to check if it exists in the repository or not</param>
        /// <returns>True if the item is found in the repository otherwise false</returns>
        public async Task<bool> ContainsAsync(TEntity item)
        {
            return await DbSet.ContainsAsync(item);
        }

        /// <summary>
        /// Get all elements of type {T} in repository
        /// </summary>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageCount">Number of elements in each page</param>
        /// <param name="orderByExpression">Order by expression for this query</param>
        /// <param name="ascending">Specify if order is ascending</param>
        /// <returns>List of selected elements</returns>
        public IEnumerable<TEntity> Query<TOrderBy>(int pageIndex, int pageCount,
            Expression<Func<TEntity, TOrderBy>> orderByExpression, bool ascending)
        {
            return (ascending)
                ? this.DbSet
                    .OrderBy(orderByExpression)
                    .Skip(pageIndex * pageCount)
                    .Take(pageCount)
                    .ToList()
                : this.DbSet
                    .OrderByDescending(orderByExpression)
                    .Skip(pageIndex * pageCount)
                    .Take(pageCount)
                    .ToList();
        }

        /// <summary>
        /// Get all elements of type {T} in repository in async
        /// </summary>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageCount">Number of elements in each page</param>
        /// <param name="orderByExpression">Order by expression for this query</param>
        /// <param name="ascending">Specify if order is ascending</param>
        /// <returns>List of selected elements</returns>
        public async Task<List<TEntity>> QueryAsync<TOrderBy>(int pageIndex, int pageCount, Expression<Func<TEntity, TOrderBy>> orderByExpression, bool @ascending)
        {
            return await ((ascending)
               ? this.DbSet
                   .OrderBy(orderByExpression)
                   .Skip(pageIndex * pageCount)
                   .Take(pageCount).ToListAsync()
               : this.DbSet
                   .OrderByDescending(orderByExpression)
                   .Skip(pageIndex * pageCount)
                   .Take(pageCount)
                   .ToListAsync());
        }

        /// <summary>
        /// Get  elements of type {T} in repository
        /// </summary>
        /// <param name="filter">Filter that each element do match</param>
        /// <returns>List of selected elements</returns>
        public IEnumerable<TEntity> Query(Expression<Func<TEntity, bool>> filter)
        {
            return this.DbSet
                .Where(filter)
                .ToList();
        }

        /// <summary>
        /// Get  elements of type {T} in repository async
        /// </summary>
        /// <param name="filter">Filter that each element do match</param>
        /// <returns>List of selected elements</returns>
        public async Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await this.DbSet.Where(filter).ToListAsync();
        }

        /// <summary>
        /// Get all elements of type {T} in repository after apply the filter 
        /// </summary>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageCount">Number of elements in each page</param>
        /// <param name="orderByExpression">Order by expression for this query</param>
        /// <param name="ascending">Specify if order is ascending</param>
        /// <param name="filter">filter to be apply</param>
        /// <returns>List of selected elements</returns>
        public IEnumerable<TEntity> Query<TOrderBy>(Expression<Func<TEntity, bool>> filter, int pageIndex, int pageCount, Expression<Func<TEntity, TOrderBy>> orderByExpression,
            bool @ascending)
        {
            return (ascending)
                               ?
                                   this.DbSet
                                    .Where(filter)
                                    .OrderBy(orderByExpression)
                                       .Skip(pageIndex * pageCount)
                                    .Take(pageCount)
                                    .ToList()
                               :
                                   this.DbSet
                                    .Where(filter)
                                    .OrderByDescending(orderByExpression)
                                       .Skip(pageIndex * pageCount)
                                    .Take(pageCount)
                                    .ToList();
        }

        /// <summary>
        /// Get all elements of type {T} in repository after apply the filter in async
        /// </summary>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageCount">Number of elements in each page</param>
        /// <param name="orderByExpression">Order by expression for this query</param>
        /// <param name="ascending">Specify if order is ascending</param>
        /// <param name="filter">filter to be apply</param>
        /// <returns>List of selected elements</returns>
        public async Task<IEnumerable<TEntity>> QueryAsync<TOrderBy>(Expression<Func<TEntity, bool>> filter, int pageIndex, int pageCount, Expression<Func<TEntity, TOrderBy>> orderByExpression, bool @ascending)
        {
            return await ((ascending)
                              ?
                                  this.DbSet
                                   .Where(filter)
                                   .OrderBy(orderByExpression)
                                      .Skip(pageIndex * pageCount)
                                   .Take(pageCount).ToListAsync()
                              :
                                  this.DbSet
                                   .Where(filter)
                                   .OrderByDescending(orderByExpression)
                                      .Skip(pageIndex * pageCount)
                                   .Take(pageCount)
                                   .ToListAsync());
        }

        #endregion


        public async Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var item = await SourceDbSet.SingleOrDefaultAsync(predicate);
            return item == null ? null : item;
        }

        public TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            var item = SourceDbSet.SingleOrDefault(predicate);
            return item == null? null : item;
        }

        public async Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var item = await SourceDbSet.SingleAsync(predicate);
            return item == null ? null : item;
        }
        public TEntity Single(Expression<Func<TEntity, bool>> predicate)
        {
            var item = SourceDbSet.Single(predicate);
            return item == null? null : item;
        }

        #region Implementation of IKeyedRepository<TEntity,in TKey>

        /// <summary>
        /// Get Entity by Id
        /// </summary>
        /// <param name="id">Id(Key) of the entity</param>
        /// <returns></returns>
        public TEntity Get(TKey id)
        {
            var item = SourceDbSet.Find(id);
            return item == null? null : item;
        }

        public async Task<TEntity> GetAsync(TKey id)
        {
            var item = await SourceDbSet.FindAsync(id);
            return item == null  ? null : item;
        }
        #endregion

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                DbContext.Dispose();
            }
            _disposed = true;
        }
    }
}
