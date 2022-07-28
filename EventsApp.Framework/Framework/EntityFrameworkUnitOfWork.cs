using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EventsApp.Framework
{
    public class EntityFrameworkUnitOfWork : IUnitOfWork
    {
        #region Fields

        private readonly Guid _instanceId;

        private DbContext _context;
        /// <summary>
        /// inner TransactionScope Object
        /// </summary>
        private bool _disposed = false;

        #endregion Fields
        public Guid InstanceId
        {
            get { return _instanceId; }
        }
        #region C-tor
        public EntityFrameworkUnitOfWork()
        {
            _instanceId = Guid.NewGuid();
        }
        public EntityFrameworkUnitOfWork(System.Data.Entity.DbContext context)
        {
            _context = context;
            _instanceId = Guid.NewGuid();
        }

        #endregion C-tor

        public void Commit()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }
        public Task<int> CommitAsync()
        {
            return _context.SaveChangesAsync();
        }
        public void Rollback()
        {
            //TODO:reset the context status
        }

        public void Save<TEntity>(TEntity item) where TEntity : class, new()
        {
            var dbSet = _context.Set<TEntity>();
            dbSet.Add(item);
        }

        public void Delete<TEntity>(TEntity item) where TEntity : class, new()
        {
            var dbSet = _context.Set<TEntity>();
            if (_context.Entry(item).State == EntityState.Detached)
            {
                dbSet.Attach(item);
            }
            dbSet.Remove(item);
        }

        public IQueryable<TEntity> EntitySet<TEntity>() where TEntity : class
        {
            var dbSet = _context.Set<TEntity>();
            return dbSet;
        }

        public TContext OrmContext<TContext>() where TContext : class
        {
            return _context as TContext;
        }


        #region Implementation of IDispose

        /// <summary>
        /// Finalizer method.  If Dispose() is called correctly, there is nothing
        /// for this to do.
        /// </summary>
        ~EntityFrameworkUnitOfWork()
        {
            Dispose(false);
        }

        /// <summary>
        /// Implement IDisposable.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            // This object will be cleaned up by the Dispose method.
            // take this object off the finalization queue and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public Task<int> SaveAsync()
        {
            return _context.SaveChangesAsync();
        }

        public Task<int> SaveAsync(CancellationToken cancellationToken)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Performs the actual dispose of the object
        /// </summary>
        /// <param name="disposing">If disposing equals true, the method has been called directly
        /// or indirectly by a user's code. Managed and unmanaged resources can be disposed.
        /// If disposing equals false, the method has been called by the
        /// runtime from inside the finalizer and you should not reference
        /// other objects. Only unmanaged resources can be disposed.</param>
        public void Dispose(bool disposing)
        {
            try
            {
                // Check to see if Dispose has already been called.
                if (!this._disposed)
                {
                    // If disposing equals true, dispose all managed
                    // and unmanaged resources.
                    if (disposing)
                    {
                        // Dispose managed resources.
                        //_transactionScope.Dispose();
                        _context.Dispose();
                        _context = null;
                    }

                    // Call the appropriate methods to clean up
                    // unmanaged resources here.


                }
            }
            catch (Exception ex)
            {
              //  ApplicationContext.Instance.Logger.Error(ex);
            }
            finally
            {
                // Note disposing has been done.
                this._disposed = true;
            }
        }


        #endregion






    }
}
