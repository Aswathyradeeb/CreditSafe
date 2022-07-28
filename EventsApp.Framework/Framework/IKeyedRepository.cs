using System;
using System.Threading.Tasks;

namespace EventsApp.Framework
{
    /// <summary>
    /// Interface for repository  f an entity with Id of type TKey
    /// </summary>
    public interface IKeyedRepository<TEntity, in TKey> : IRepository<TEntity>
        where TEntity : class
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Get Entity by Id
        /// </summary>
        /// <param name="id">Id(Key) of the entity</param>
        /// <returns></returns>
        TEntity Get(TKey id);


        /// <summary>
        /// Get Entity by Id Async
        /// </summary>
        /// <param name="id">Id(Key) of the entity</param>
        /// <returns></returns>
        Task<TEntity> GetAsync(TKey id);

    }
}
