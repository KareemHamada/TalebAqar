
namespace BLL.Repositories
{
    public class CurrencyRepository : GenericRepository<TbCurrency>, ICurrencyRepository
    {
        public CurrencyRepository(RealEstateContext dbContext) : base(dbContext)
        {

        }
    }
}
