namespace BLL.Repositories
{
    public class OwnerRepository : GenericRepository<TbOwner>, IOwnerRepository
    {
        public OwnerRepository(RealEstateContext dbContext) : base(dbContext)
        {

        }
    }
}
