namespace DAL.Data.Configurations
{
    public partial class TbAddressConfiguration : IEntityTypeConfiguration<TbAddress>
    {
        public void Configure(EntityTypeBuilder<TbAddress> entity)
        {
            entity.HasKey(e => e.AddressId);

            entity.HasOne(d => d.City).WithMany(p => p.TbAddresses).HasForeignKey(f => f.CityId);

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<TbAddress> entity);
    }
}
