
namespace RealEstate.Profiles
{
	public class RealEstateProfiles : Profile
	{
		public RealEstateProfiles() {

			CreateMap<TbCity, CityVM>().ReverseMap();
			CreateMap<TbAddress, AddressVM>().ReverseMap();
			CreateMap<TbStatus, StatusVM>().ReverseMap();
			CreateMap<TbOwner, OwnerVM>().ReverseMap();
			CreateMap<TbType, TypeVM>().ReverseMap();
			CreateMap<TbProperty, PropertyVM>().ReverseMap();
            CreateMap<TbGovernorate, GovernorateVM>().ReverseMap();
            CreateMap<TbSetting, SettingVM>().ReverseMap();
            CreateMap<TbCurrency, CurrencyVM>().ReverseMap();

            //CreateMap<TbProperty, PropertyDTO>().ReverseMap();

        }
    }
}
