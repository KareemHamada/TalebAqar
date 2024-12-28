namespace BLL.Repositories
{
    public class OwnerRepository : GenericRepository<TbOwner>, IOwnerRepository
    {
        public OwnerRepository(RealEstateContext dbContext) : base(dbContext)
        {

        }



        public async Task<ICollection<TbOwner>> GetAllOwnersAsync() => await _dbSet.AsNoTracking().Where(x => x.CurrentState == true).OrderByDescending(x=>x.OwnerId).ToListAsync();

    }
}
