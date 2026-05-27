using Microsoft.EntityFrameworkCore;
using Unalive_WebManagement.DAL.Interfaces;
using Unalive_WebManagement.Data;

namespace Unalive_WebManagement.DAL.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly UnaliveDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(UnaliveDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public virtual async Task<T?> GetByIdAsync(params object[] keyValues)
        {
            var entity = await _dbSet.FindAsync(keyValues);
            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Detached;
            }
            return entity;
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task UpdateAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(params object[] keyValues)
        {
            var entity = await GetByIdAsync(keyValues);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public virtual async Task<bool> ExistsAsync(params object[] keyValues)
        {
            var entity = await GetByIdAsync(keyValues);
            return entity != null;
        }
    }
}

