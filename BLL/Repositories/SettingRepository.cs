namespace BLL.Repositories
{
    public class SettingRepository : GenericRepository<TbSetting>, ISettingRepository
    {
        public SettingRepository(RealEstateContext dbContext) : base(dbContext)
        {

        }
    }

}
