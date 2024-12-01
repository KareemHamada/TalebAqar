namespace DAL.Data.Configurations
{
    public partial class TbTypeConfiguration : IEntityTypeConfiguration<TbType>
    {
        public void Configure(EntityTypeBuilder<TbType> entity)
        {
            entity.HasKey(e => e.TypeId);

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<TbType> entity);
    }
}
