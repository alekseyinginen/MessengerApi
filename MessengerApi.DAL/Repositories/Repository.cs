using MessengerApi.DAL.EF;
using MessengerApi.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessengerApi.DAL.Repositories
{
    class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class
    {
        protected readonly ApplicationContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public Repository(ApplicationContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public IQueryable<TEntity> Query()
        {
            return _dbSet;
        }

        public IEnumerable<TEntity> Get(Func<TEntity, bool> predicate)
        {
            return _dbSet.Where(predicate).ToList();
        }

        public Task<TEntity> GetById(TKey id) {
            return _dbSet.FindAsync(id);
        }

        public IEnumerable<TEntity> GetAll() {
            return _dbSet;
        }

        public IEnumerable<TEntity> GetRange(int page, int itemsPerPage) {
            return _dbSet.Skip(page * itemsPerPage).Take(itemsPerPage);
        }

        public async Task<TEntity> Create(TEntity item)
        {
            TEntity entity = _dbSet.Add(item).Entity;
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<TEntity> Update(TEntity item)
        {
            TEntity entity = _dbSet.Update(item).Entity;
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<TEntity> Delete(TKey id)
        {
            TEntity entity = _dbSet.Remove(_dbSet.Find(id)).Entity;
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
