namespace DAL.Data.Configurations
{
    public partial class TbSettingConfiguration : IEntityTypeConfiguration<TbSetting>
    {
        public void Configure(EntityTypeBuilder<TbSetting> entity)
        {
            entity.HasKey(e => e.SettingId);

            entity.Property(e => e.CurrentState).HasDefaultValue(true);



            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<TbSetting> entity);
    }
}
