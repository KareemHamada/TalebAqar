namespace BLL.Interfaces
{
    public interface IUnitOfWork
    {
        public IAddressRepository Addresses { get; }
        public ICityRepository Cities { get; }
        public IGovernorateRepository Governorates { get; }
        public IOwnerRepository Owners { get; }
        public IPropertyRepository Properties { get; }
        public IStatusRepository Statuses { get; }
        public ITypeRepository Types { get; }
		public IPropertyImagesRepository PropertyImages { get; }
        public ISettingRepository Settings { get; }

        public Task<int> SaveChangesAsync();

    }
}
