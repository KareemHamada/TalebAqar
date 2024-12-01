namespace BLL.Repositories
{
    public class TypeRepository : GenericRepository<TbType>, ITypeRepository
    {
        public TypeRepository(RealEstateContext dbContext) : base(dbContext)
        {

        }
    }
}