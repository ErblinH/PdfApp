using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PdfApp.Data.Extensions;
using PdfApp.Domain.Caching;
using PdfApp.Domain.Entities;
using PdfApp.Domain.PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace PdfApp.Data.Repositories
{
    /// <summary>
    /// Represents the entity repository implementation
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    public partial class EntityRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        #region Fields

        private readonly ILogger<EntityRepository<TEntity>> _logger;
        private readonly PdfAppDbContext _databaseContext;
        protected readonly DbSet<TEntity> _table;
        private readonly IStaticCacheManager _staticCacheManager;

        #endregion

        #region Ctor

        public EntityRepository(
            ILogger<EntityRepository<TEntity>> logger,
            IStaticCacheManager staticCacheManager,
            PdfAppDbContext databaseContext)
        {
            _logger = logger;
            _databaseContext = databaseContext;
            _table = _databaseContext.Set<TEntity>();
            _staticCacheManager = staticCacheManager;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Get all entity entries
        /// </summary>
        /// <param name="getAllAsync">Function to select entries</param>
        /// <param name="getCacheKey">Function to get a cache key; pass null to don't cache; return null from this function to use the default key</param>
        /// <returns>Entity entries</returns>
        protected virtual async Task<IList<TEntity>> GetEntities(Func<Task<IList<TEntity>>> getAllAsync, Func<IStaticCacheManager, CacheKey> getCacheKey)
        {
            if (getCacheKey == null)
                return await getAllAsync();

            //caching
            var cacheKey = getCacheKey(_staticCacheManager)
                           ?? _staticCacheManager.PrepareKeyForDefaultCache(PdfAppEntityCacheDefaults<TEntity>.AllCacheKey);
            return await _staticCacheManager.GetAsync(cacheKey, getAllAsync);
        }

        /// <summary>
        /// Get all entity entries
        /// </summary>
        /// <param name="getAllAsync">Function to select entries</param>
        /// <param name="getCacheKey">Function to get a cache key; pass null to don't cache; return null from this function to use the default key</param>
        /// <returns>Entity entries</returns>
        protected virtual async Task<IList<TEntity>> GetEntities(Func<Task<IList<TEntity>>> getAllAsync, Func<IStaticCacheManager, Task<CacheKey>> getCacheKey)
        {
            if (getCacheKey == null)
                return await getAllAsync();

            //caching
            var cacheKey = await getCacheKey(_staticCacheManager)
                           ?? _staticCacheManager.PrepareKeyForDefaultCache(PdfAppEntityCacheDefaults<TEntity>.AllCacheKey);
            return await _staticCacheManager.GetAsync(cacheKey, getAllAsync);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get the entity entry
        /// </summary>
        /// <param name="func">Function to select entry</param>
        /// <param name="getCacheKey">Function to get a cache key; pass null to don't cache; return null from this function to use the default key</param>
        /// <returns>Entity entry</returns>
        public virtual async Task<TEntity> GetAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null,
            Func<IStaticCacheManager, CacheKey> getCacheKey = null)
        {
            try
            {
                async Task<TEntity> GetEntityAsync()
                {
                    var query = func != null ? func(Table) : Table;
                    query = query.Where(x => !x.Deleted);
                    return await query.FirstOrDefaultAsync();
                }

                if (getCacheKey == null)
                    return await GetEntityAsync();

                //caching
                var cacheKey = getCacheKey(_staticCacheManager)
                               ?? _staticCacheManager.PrepareKeyForDefaultCache(PdfAppEntityCacheDefaults<TEntity>
                                   .AllCacheKey);

                return await _staticCacheManager.GetAsync(cacheKey, GetEntityAsync);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Get all entity entries
        /// </summary>
        /// <param name="func">Function to select entries</param>
        /// <param name="getCacheKey">Function to get a cache key; pass null to don't cache; return null from this function to use the default key</param>
        /// <returns>Entity entries</returns>
        public virtual async Task<IList<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null,
            Func<IStaticCacheManager, CacheKey> getCacheKey = null)
        {
            async Task<IList<TEntity>> getAllAsync()
            {
                var query = func != null ? func(Table) : Table;
                query = query.Where(x => !x.Deleted);
                return await query.ToListAsync();
            }

            return await GetEntities(getAllAsync, getCacheKey);
        }

        /// <summary>
        /// Get paged list of all entity entries
        /// </summary>
        /// <param name="func">Function to select entries</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="getOnlyTotalCount">Whether to get only the total number of entries without actually loading data</param>
        /// <returns>Paged list of entity entries</returns>
        public virtual async Task<IPagedList<TEntity>> GetAllPagedAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null,
            int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false)
        {
            var query = func != null ? func(Table) : Table;

            query = query.Where(x => !x.Deleted);

            return await query.ToPagedListAsync(pageIndex, pageSize, getOnlyTotalCount);
        }

        /// <summary>
        /// Insert the entity entry
        /// </summary>
        /// <param name="entity">Entity entry</param>
        /// <param name="publishEvent">Whether to publish event notification</param>
        public virtual async Task<bool> InsertAsync(TEntity entity, bool publishEvent = true)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));


            try
            {
                await _table.AddAsync(entity);
                return await _databaseContext.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be added: {ex.Message} and {ex.InnerException}");
            }

            //event notification
            //if (publishEvent)
            //    await _eventPublisher.EntityInsertedAsync(entity);
        }

        /// <summary>
        /// Update the entity entry
        /// </summary>
        /// <param name="entity">Entity entry</param>
        /// <param name="publishEvent">Whether to publish event notification</param>
        public virtual void Update(TEntity entity, bool publishEvent = true)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            try
            {
                _table.Update(entity);
                _databaseContext.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new Exception($"{typeof(TEntity)} could not be updated: {e.Message} and {e.InnerException}");
            }

            //event notification
            //if (publishEvent)
            //    await _eventPublisher.EntityUpdatedAsync(entity);
        }

        /// <summary>
        /// Update entity async
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task UpdateAsync(TEntity entity)
        {
            _databaseContext.Entry(entity).State = EntityState.Modified;
            await _databaseContext.SaveChangesAsync();
        }


        /// <summary>
        /// Execute function. Be extra care when using this function as there is a risk for SQL injection
        /// </summary>
        public T GetFromStoredProcedure<T>(string storedProcedure, List<SqlParameter> sqlParameters) where T : class
        {
            try
            {
                return _databaseContext.Set<T>().FromSqlRaw(storedProcedure, sqlParameters.ToArray()).AsEnumerable().FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return null;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a table
        /// </summary>
        public virtual IQueryable<TEntity> Table => _table;

        #endregion
    }
}
