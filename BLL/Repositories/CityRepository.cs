namespace BLL.Repositories
{
    public class CityRepository : GenericRepository<TbCity>, ICityRepository
    {
        public CityRepository(RealEstateContext dbContext) : base(dbContext)
        {

        }



		public async Task<IEnumerable<TbCity>> GetAllWithGovernorateAsync() => await _dbSet.Where(x=>x.CurrentState == true).Include(a => a.Governorate).ToListAsync();

	}
}
