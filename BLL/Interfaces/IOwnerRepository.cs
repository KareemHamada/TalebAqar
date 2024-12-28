namespace BLL.Interfaces
{
    public interface IOwnerRepository : IGenericRepository<TbOwner>
    {
        Task<ICollection<TbOwner>> GetAllOwnersAsync();

    }
}
