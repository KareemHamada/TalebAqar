namespace BLL.Interfaces
{
    public interface ICityRepository : IGenericRepository<TbCity>
    {

		public Task<IEnumerable<TbCity>> GetAllWithGovernorateAsync();
	}
}

