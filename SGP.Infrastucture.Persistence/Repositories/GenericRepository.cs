using Microsoft.EntityFrameworkCore;
using SGP.Core.Application.Interfaces.Repositories;
using SGP.Infrastucture.Persistence.Contexts;

namespace SGP.Infrastucture.Persistence.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly ApplicationContext _dbContext;

        public GenericRepository(ApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            await _dbContext.Set<TEntity>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            _dbContext.Set<TEntity>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task<List<TEntity>> GetAllAsync()
        {
            return await _dbContext.Set<TEntity>().ToListAsync();
        }

        public virtual async Task<TEntity> GetByIdAsync(int id)
        {
            return await _dbContext.Set<TEntity>().FindAsync(id);
        }

        public virtual async Task<List<TEntity>> GetAllWithIncludeAsync(List<string> properties)
        {
            var query = _dbContext.Set<TEntity>().AsQueryable();
            foreach (string property in properties)
            {
                query = query.Include(property);
            }
            return await query.ToListAsync();
        }
    }
}
