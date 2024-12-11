namespace BLL.Repositories
{


    public class PropertyRepository : GenericRepository<TbProperty>, IPropertyRepository
    {
		private readonly RealEstateContext _dataContext;

		public PropertyRepository(RealEstateContext dbContext) : base(dbContext)
        {
			_dataContext = dbContext;

		}


        public async Task<TbProperty?> GetAsync(int id) => await _dbSet.Where(x=>x.PropertyId == id && x.CurrentState == true).FirstOrDefaultAsync();


        public async Task<IEnumerable<TbProperty>> GetAllWithNamesAsync() => await _dbSet.Where(x => x.CurrentState == true).Include(p => p.Address)
               .Include(p => p.City)
               .Include(p => p.Governorate)
               .Include(p => p.Owner)
               .Include(p => p.Status)
               .Include(p => p.PropertyImages)
               .Include(p => p.Type)
		       .ToListAsync();




		public async Task<TbProperty?> GetWithNamesAsync(int id) => await _dbSet.Where(x => x.PropertyId == id && x.CurrentState == true)
				.Include(p => p.Address)
				.Include(p => p.City)
				.Include(p => p.Governorate)
				.Include(p => p.Owner)
				.Include(p => p.Status)
				.Include(p => p.PropertyImages)
				.Include(p => p.CreatedByUser)
			   .Include(p => p.UpdatedByUser)
				.Include(p => p.Type).FirstOrDefaultAsync();


		public async Task<TbProperty> AddAsync(TbProperty property)
		{

			property.CurrentState = true;
			// Set CustomAutoIncrementColumn to the next value

			var maxNumOfAdvertisement = await _dbSet.MaxAsync(p => (int?)p.NumOfAdvertisement) ?? 1000;
			property.NumOfAdvertisement = maxNumOfAdvertisement + 1;


			await _dbSet.AddAsync(property);
			await _dataContext.SaveChangesAsync();

			return property;
		}

        public async Task<IEnumerable<TbProperty>> FeaturedPropertiesAsync(int count) => await _dbSet.Where(x => x.CurrentState == true)
                .OrderBy(p => Guid.NewGuid())  // Randomize order
				.Take(count)
                .Include(p => p.Address)
				.Include(p => p.City)
				.Include(p => p.Governorate)
				.Include(p => p.Owner)
				.Include(p => p.Status)
				.Include(p => p.PropertyImages)
				.Include(p => p.Type)
				.ToListAsync();

        public async Task<IEnumerable<TbProperty>> LatestPropertiesAsync(int count) => await _dbSet.Where(x => x.CurrentState == true)
                .OrderByDescending(p => p.CreatedDate)  // Randomize order
                .Take(count)
                .Include(p => p.Address)
                .Include(p => p.City)
                .Include(p => p.Governorate)
                .Include(p => p.Owner)
                .Include(p => p.Status)
                .Include(p => p.PropertyImages)
                .Include(p => p.Type)
                .ToListAsync();


        public async Task<IEnumerable<TbProperty>> PropertiesInTheSameGovernorate(int? govornorate) => await _dbSet.Where(x => x.GovernorateId == govornorate && x.CurrentState == true)
               .Take(5)
               .Include(p => p.Address)
               .Include(p => p.City)
               .Include(p => p.Governorate)
               .Include(p => p.Owner)
               .Include(p => p.Status)
               .Include(p => p.PropertyImages)
               .Include(p => p.Type)
               .ToListAsync();

    }
}