using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Test.Platform.Wms.Core.Interfaces;

namespace Test.Platform.Wms.Sql.Abstractions
{
    public class AbstractDbContextCrudRepo<TEntity, TDbContext> : ICrudRepo<TEntity>
        where TDbContext : DbContext
        where TEntity : class
    {
        private readonly TDbContext _context;

        public AbstractDbContextCrudRepo(TDbContext context)
        {
            _context = context;
        }

        public async Task<TEntity> CreateAsync(Guid key, TEntity entity, CancellationToken cancellationToken)
        {
            await _context.AddAsync<TEntity>(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken)
        {
            var list = await _context.Set<TEntity>().ToListAsync(cancellationToken);

            return list;
        }

        public Task<TEntity> GetByKeyAsync(Guid key, CancellationToken cancellationToken)
        {
            return _context.FindAsync<TEntity>(new object[] { key }, cancellationToken).AsTask();
        }
    }
}