namespace DAL.Data.Configurations
{
	public partial class TbPropertyConfiguration : IEntityTypeConfiguration<TbProperty>
	{
		public void Configure(EntityTypeBuilder<TbProperty> entity)
		{
			entity.HasKey(e => e.PropertyId);
			entity.Property(e => e.Negotiable).HasDefaultValue(true);
			entity.Property(e => e.IsSoldOrRenteled).HasDefaultValue(false);

			entity.Property(e=>e.NumOfAdvertisement).HasDefaultValue(1000);
            entity.Property(e => e.NumOfViews).HasDefaultValue(0);

              entity.HasOne(d => d.Address).WithMany(p => p.TbProperties).HasForeignKey(f => f.AddressId);

			entity.HasOne(d => d.City).WithMany(p => p.TbProperties).HasForeignKey(f => f.CityId);

			entity.HasOne(d => d.Governorate).WithMany(p => p.TbProperties).HasForeignKey(f => f.GovernorateId);

			entity.HasOne(d => d.Owner).WithMany(p => p.TbProperties).HasForeignKey(f => f.OwnerId);

			entity.HasOne(d => d.Status).WithMany(p => p.TbProperties).HasForeignKey(f => f.StatusId);

			entity.HasOne(d => d.Type).WithMany(p => p.TbProperties).HasForeignKey(f => f.TypeId);

            entity.HasOne(d => d.Currency).WithMany(p => p.TbProperties).HasForeignKey(f => f.CurrencyId);


            entity.HasOne(p => p.CreatedByUser).WithMany().HasForeignKey(p => p.CreatedBy);

			entity.HasOne(p => p.UpdatedByUser).WithMany().HasForeignKey(p => p.UpdatedBy);


			OnConfigurePartial(entity);
		}

		partial void OnConfigurePartial(EntityTypeBuilder<TbProperty> entity);
	}
}
