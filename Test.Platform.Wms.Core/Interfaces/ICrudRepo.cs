using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Test.Platform.Wms.Core.Interfaces
{
    public interface ICrudRepo<TEntity>
        where TEntity:IEntity
    {
         Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken);
         
         Task<TEntity> GetByKeyAsync(Guid key, CancellationToken cancellationToken);

         Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken);

         Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken);

         Task DeleteAsync(Guid key, CancellationToken cancellationToken);
    }
}