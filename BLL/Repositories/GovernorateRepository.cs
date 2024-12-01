namespace BLL.Repositories
{
    public class GovernorateRepository : GenericRepository<TbGovernorate>, IGovernorateRepository
    {
        public GovernorateRepository(RealEstateContext dbContext) : base(dbContext)
        {

        }
    }
}
