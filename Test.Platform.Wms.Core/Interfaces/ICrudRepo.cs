using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Test.Platform.Wms.Core.Interfaces
{
    public interface ICrudRepo<T>
    {
         Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken);
         
         Task<T> GetByKeyAsync(Guid key, CancellationToken cancellationToken);

         Task<T> CreateAsync(Guid key, T entity, CancellationToken cancellationToken);
    }
}