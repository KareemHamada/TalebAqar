namespace BLL.Repositories
{
    public class StatusRepository : GenericRepository<TbStatus>, IStatusRepository
    {
        public StatusRepository(RealEstateContext dbContext) : base(dbContext)
        {

        }
    }
}