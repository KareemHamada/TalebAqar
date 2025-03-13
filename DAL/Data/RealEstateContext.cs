
namespace DAL.Data
{
    public partial class RealEstateContext : IdentityDbContext<ApplicationUser>
	{

        public RealEstateContext(DbContextOptions<RealEstateContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TbAddress> TbAddresses { get; set; }

        public virtual DbSet<TbCity> TbCities { get; set; }

        public virtual DbSet<TbGovernorate> TbGovernorates { get; set; }

        public virtual DbSet<TbOwner> TbOwners { get; set; }

        public virtual DbSet<TbProperty> TbProperties { get; set; }

        public virtual DbSet<TbPropertyImage> TbPropertyImages { get; set; }

        public virtual DbSet<TbStatus> TbStatuses { get; set; }

        public virtual DbSet<TbType> TbTypes { get; set; }
        public virtual DbSet<TbSetting> TbSettings { get; set; }
        public virtual DbSet<TbCurrency> TbCurrencies { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
			base.OnModelCreating(modelBuilder);

			modelBuilder.ApplyConfiguration(new Configurations.TbAddressConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.TbCityConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.TbGovernorateConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.TbOwnerConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.TbPropertyConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.TbPropertyImageConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.TbStatusConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.TbTypeConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.TbSettingConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.TbCurrencyConfiguration());

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }

}

