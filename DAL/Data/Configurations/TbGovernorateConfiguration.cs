namespace DAL.Data.Configurations
{
    public partial class TbGovernorateConfiguration : IEntityTypeConfiguration<TbGovernorate>
    {
        public void Configure(EntityTypeBuilder<TbGovernorate> entity)
        {
            entity.HasKey(e => e.GovernorateId);

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<TbGovernorate> entity);
    }
}
