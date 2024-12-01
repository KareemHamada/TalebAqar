using DAL.Data;
using DAL.Interfaces;

namespace BLL.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class, IHasCurrentState
	{
        private RealEstateContext _dbContext;
        protected DbSet<TEntity> _dbSet;
        public GenericRepository(RealEstateContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }
        public async Task<TEntity> AddAsync(TEntity entity) {

			entity.CurrentState = true;
			await _dbSet.AddAsync(entity);

            return entity;  
		} 


        public void Delete(TEntity entity) {
            entity.CurrentState = false;
			_dbSet.Update(entity);
		} 

        public async Task<TEntity?> GetAsync(int id) => await _dbSet.FindAsync(id);

        public async Task<ICollection<TEntity>> GetAllAsync() =>   await _dbSet.AsNoTracking().Where(x => x.CurrentState == true).ToListAsync();

        public void Update(TEntity entity) => _dbSet.Update(entity);

        public void Detach(TEntity entity) => _dbContext.Entry(entity).State = EntityState.Detached;

    }
}
