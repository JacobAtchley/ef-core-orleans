using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Test.Platform.Wms.Core.Extensions;
using Test.Platform.Wms.Core.Interfaces;

namespace Test.Platform.Wms.EntityFramework.Abstractions
{
    public class AbstractDbContextCrudRepo<TEntity, TDbContext> : ICrudRepo<TEntity>
        where TDbContext : DbContext
        where TEntity : class, IEntity, new()
    {
        protected readonly TDbContext Context;
        
        private DbSet<TEntity> GetDbSet()
        {
            return Context.Set<TEntity>();
        }

        public AbstractDbContextCrudRepo(TDbContext context)
        {
            Context = context;
        }

        public async Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            var entry = await GetDbSet().AddAsync(entity, cancellationToken).ConfigureAwait(false);

            await Context.SaveChangesAsync(cancellationToken);

            return entry.Entity;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            var entry = Context.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                var set = GetDbSet();

                var found = await set.FindAsync(entity.Id);

                if (found != null)
                {
                    entity.CopyPropertiesTo(found, "Id");
                }
                else
                {
                    set.Attach(entity);
                    entry.State = EntityState.Modified;
                }

            }

            await Context.SaveChangesAsync(cancellationToken);

            return entry.Entity;
        }

        public async Task DeleteAsync(Guid key, CancellationToken cancellationToken)
        {
            var entity = new TEntity
            {
                Id = key
            };

            var entry = Context.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                var set = GetDbSet();

                var found = await set.FindAsync(key);

                if (found != null)
                {
                    set.Remove(found);
                }
                else
                {
                    set.Attach(entity);
                    entry.State = EntityState.Deleted;
                }

            }

            await Context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken)
        {
            var list = await GetDbSet().ToListAsync(cancellationToken);

            return list;
        }

        public Task<TEntity> GetByKeyAsync(Guid key, CancellationToken cancellationToken)
        {
            return GetDbSet().FindAsync(new object[] { key }, cancellationToken).AsTask();
        }
    }
}