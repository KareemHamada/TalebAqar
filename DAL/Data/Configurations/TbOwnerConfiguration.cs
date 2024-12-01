namespace DAL.Data.Configurations
{
    public partial class TbOwnerConfiguration : IEntityTypeConfiguration<TbOwner>
    {
        public void Configure(EntityTypeBuilder<TbOwner> entity)
        {
            entity.HasKey(e => e.OwnerId);

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<TbOwner> entity);
    }
}
