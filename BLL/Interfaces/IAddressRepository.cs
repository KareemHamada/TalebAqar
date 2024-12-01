namespace BLL.Interfaces
{
    public interface IAddressRepository : IGenericRepository<TbAddress>
    {
		public Task<IEnumerable<TbAddress>> GetAllWithCityAsync();
	}
}
