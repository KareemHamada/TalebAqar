namespace BLL.Repositories
{
	public class PropertyImagesRepository : GenericRepository<TbPropertyImage>, IPropertyImagesRepository
	{
		public PropertyImagesRepository(RealEstateContext dbContext) : base(dbContext)
		{

		}
	}
}
