namespace BLL.Repositories
{
    public class AddressRepository : GenericRepository<TbAddress>, IAddressRepository
    {
        public AddressRepository(RealEstateContext dbContext) : base(dbContext)
        {

        }


		public async Task<IEnumerable<TbAddress>> GetAllWithCityAsync() => await _dbSet.Where(x => x.CurrentState == true).Include(a => a.City).ToListAsync();

	}
}
