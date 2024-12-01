
namespace BLL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Lazy<IAddressRepository> _addressRepository;
        private readonly Lazy<ICityRepository> _cityRepository;
        private readonly Lazy<IGovernorateRepository> _governorateRepository;
        private readonly Lazy<IOwnerRepository> _ownerRepository;
        private readonly Lazy<IPropertyRepository> _propertyRepository;
        private readonly Lazy<IStatusRepository> _statusRepository;
        private readonly Lazy<ITypeRepository> _typeRepository;
		private readonly Lazy<IPropertyImagesRepository> _propertyImagesRepository;
        private readonly Lazy<ISettingRepository> _settingRepository;


        private readonly RealEstateContext _dataContext;

        public UnitOfWork(RealEstateContext dataContext)
        {
            _addressRepository = new Lazy<IAddressRepository>(() => new AddressRepository(dataContext));

            _cityRepository = new Lazy<ICityRepository>(() => new CityRepository(dataContext));

            _governorateRepository = new Lazy<IGovernorateRepository>(() => new GovernorateRepository(dataContext));

            _ownerRepository = new Lazy<IOwnerRepository>(() => new OwnerRepository(dataContext));

            _propertyRepository = new Lazy<IPropertyRepository>(() => new PropertyRepository(dataContext));

            _statusRepository = new Lazy<IStatusRepository>(() => new StatusRepository(dataContext));

            _typeRepository = new Lazy<ITypeRepository>(() => new TypeRepository(dataContext));

			_propertyImagesRepository = new Lazy<IPropertyImagesRepository>(() => new PropertyImagesRepository(dataContext));

            _settingRepository = new Lazy<ISettingRepository>(() => new SettingRepository(dataContext));

            _dataContext = dataContext;
        }

        public IAddressRepository Addresses => _addressRepository.Value;
        public ICityRepository Cities => _cityRepository.Value;
        public IGovernorateRepository Governorates => _governorateRepository.Value;
        public IOwnerRepository Owners => _ownerRepository.Value;
        public IPropertyRepository Properties => _propertyRepository.Value;
        public IStatusRepository Statuses => _statusRepository.Value;
        public ITypeRepository Types => _typeRepository.Value;

		public IPropertyImagesRepository PropertyImages => _propertyImagesRepository.Value;
        public ISettingRepository Settings => _settingRepository.Value;

        public async Task<int> SaveChangesAsync() => await _dataContext.SaveChangesAsync();
    }
}
