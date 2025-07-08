using Microsoft.EntityFrameworkCore;
using PatientManager.Core.Application.Interfaces.Repositories;
using PatientManager.Infrastructure.Persistence.Contexts;

namespace PatientManager.Infrastructure.Persistence.Repositories
{
    public class GenericRepository<Entity> : IGenericRepository<Entity> where Entity : class
    {
        private readonly ApplicationDbContext _databaseContext;

        public GenericRepository(ApplicationDbContext databaseContext) => _databaseContext = databaseContext;

        public virtual async Task<Entity> AddAsync(Entity entity)
        {
            await _databaseContext.Set<Entity>().AddAsync(entity);
            await _databaseContext.SaveChangesAsync();
            return entity;
        }

        public virtual async Task UpdateAsync(Entity entity)
        {
            _databaseContext.Entry(entity).State = EntityState.Modified;
            await _databaseContext.SaveChangesAsync();
        }

        public virtual async Task RemoveAsync(Entity entity)
        {
            _databaseContext.Set<Entity>().Remove(entity);
            await _databaseContext.SaveChangesAsync();
        }

        public virtual async Task<List<Entity>> GetAllAsync() => await _databaseContext.Set<Entity>().ToListAsync();

        public virtual async Task<Entity> GetByIdAsync(int id) => await _databaseContext.Set<Entity>().FindAsync(id);

        public virtual async Task<List<Entity>> GetAllWithIncludeAsync(List<string> properties)
        {
            var query = _databaseContext.Set<Entity>().AsQueryable();

            foreach (var property in properties)
            {
                query = query.Include(property);
            }

            return await query.ToListAsync();
        }
    }
}
