namespace BLL.Interfaces
{
    public interface IPropertyRepository : IGenericRepository<TbProperty>
    {
        public Task<IEnumerable<TbProperty>> GetAllWithNamesAsync();

		public Task<TbProperty?> GetWithNamesAsync(int id);

        public Task<IEnumerable<TbProperty>> FeaturedPropertiesAsync(int count);

        public Task<IEnumerable<TbProperty>> LatestPropertiesAsync(int count);


    }
}
