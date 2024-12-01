namespace DAL.Data.Configurations
{
    public partial class TbCityConfiguration : IEntityTypeConfiguration<TbCity>
    {
        public void Configure(EntityTypeBuilder<TbCity> entity)
        {
            entity.HasKey(e => e.CityId);

            entity.HasOne(d => d.Governorate).WithMany(p => p.TbCities).HasForeignKey(f => f.GovernorateId);

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<TbCity> entity);
    }
}
