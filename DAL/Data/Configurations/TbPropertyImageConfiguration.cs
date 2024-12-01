namespace DAL.Data.Configurations
{
    public partial class TbPropertyImageConfiguration : IEntityTypeConfiguration<TbPropertyImage>
    {
        public void Configure(EntityTypeBuilder<TbPropertyImage> entity)
        {
            entity.HasKey(e => e.ImageId);

            entity.HasOne(d => d.Property).WithMany(p => p.PropertyImages).HasForeignKey(f=>f.PropertyId);

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<TbPropertyImage> entity);
    }
}
