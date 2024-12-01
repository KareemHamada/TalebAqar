namespace DAL.Data.Configurations
{
    public partial class TbStatusConfiguration : IEntityTypeConfiguration<TbStatus>
    {
        public void Configure(EntityTypeBuilder<TbStatus> entity)
        {
            entity.HasKey(e => e.StatusId);

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<TbStatus> entity);
    }
}
