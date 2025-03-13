namespace DAL.Data.Configurations
{
    public partial class TbCurrencyConfiguration : IEntityTypeConfiguration<TbCurrency>
    {
        public void Configure(EntityTypeBuilder<TbCurrency> entity)
        {
            entity.HasKey(e => e.CurrencyId);


            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<TbCurrency> entity);
    }
}
