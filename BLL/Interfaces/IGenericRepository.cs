namespace BLL.Interfaces
{
    public interface IGenericRepository<TEntity>
    {
	    Task<TEntity> AddAsync(TEntity entity);
        void Delete(TEntity entity);
        Task<TEntity?> GetAsync(int id);
        Task<ICollection<TEntity>> GetAllAsync();
        void Update(TEntity entity);

        void Detach(TEntity entity);
        
    }
}
