using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessengerApi.DAL.Interfaces
{
    public interface IRepository<TEntity, TKey> where TEntity : class
    {
        IQueryable<TEntity> Query();

        IEnumerable<TEntity> Get(Func<TEntity, bool> predicate);

        Task<TEntity> GetById(TKey id);

        IEnumerable<TEntity> GetAll();

        IEnumerable<TEntity> GetRange(int page, int itemsPerPage);

        Task<TEntity> Create(TEntity item);

        Task<TEntity> Update(TEntity item);

        Task<TEntity> Delete(TKey id);
    }
}
