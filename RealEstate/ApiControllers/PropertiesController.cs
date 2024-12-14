namespace RealEstate.ApiControllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PropertiesController : ControllerBase
	{
        private readonly RealEstateContext _realEstateContext;
        private readonly IMapper _mapper;

        public PropertiesController(RealEstateContext realEstateContext, IMapper mapper)
        {
            _realEstateContext = realEstateContext;
            _mapper = mapper;

        }

        [HttpGet("PropertiesSearchAsync")]
        public async Task<IEnumerable<PropertyDTO>> PropertiesSearchAsync([FromQuery] int? statusId,int? typeId,int? governorateId, int? cityId, int? bedrooms, int? price, string sortOrder)
        {
            var query = _realEstateContext.TbProperties.Where(x => x.CurrentState == true);

            if (statusId.HasValue)
                query = query.Where(p => p.StatusId == statusId.Value);

            if (typeId.HasValue && typeId != 0)
                query = query.Where(p => p.TypeId == typeId.Value);
            if (governorateId.HasValue && governorateId != 0)
                query = query.Where(p => p.GovernorateId == governorateId.Value);
            if (cityId.HasValue && cityId != 0)
                query = query.Where(p => p.CityId == cityId.Value);
            if (bedrooms.HasValue && bedrooms != 0)
                query = query.Where(p => p.Bedrooms == bedrooms.Value);
            if (price.HasValue)
                query = query.Where(p => p.Price <= price.Value);


            query = sortOrder switch
            {
                "price_asc" => query.OrderBy(p => p.Price),
                "price_desc" => query.OrderByDescending(p => p.Price),
                "date_asc" => query.OrderBy(p => p.CreatedDate),
                "date_desc" => query.OrderByDescending(p => p.CreatedDate),
                _ => query // Default, no sorting
            };

            return await query.Select(p => new PropertyDTO
            {
                PropertyId = p.PropertyId,
                Area = p.Area,
                Bedrooms = p.Bedrooms,
                Bathrooms = p.Bathrooms,
                Price = p.Price,
                Negotiable = p.Negotiable,
                CreatedDate = p.CreatedDate,
                Description = p.Description,
                Status = p.Status.StatusName,
                Type = p.Type.TypeName,
                Address = p.Address.AddressName,
                City = p.City.CityName,
                Governorate = p.Governorate.GovernorateName,
                Image = p.PropertyImages.FirstOrDefault().ImageUrl,
                ShortDescription = p.Description.Length > 40 ? p.Description.Substring(0, 40) + "..." : p.Description

            }).ToListAsync();
        }

        [HttpGet("GetPropertiesForSale")]
        public async Task<IEnumerable<PropertyDTO>> GetPropertiesForSale([FromQuery] string? sortOrder)
        {

            var query = _realEstateContext.TbProperties
                .Where(x => x.CurrentState == true && x.StatusId == 1)
                .Select(p => new PropertyDTO
                {
                    PropertyId = p.PropertyId,
                    Area = p.Area,
                    Bedrooms = p.Bedrooms,
                    Bathrooms = p.Bathrooms,
                    Price = p.Price,
                    Negotiable = p.Negotiable,
                    CreatedDate = p.CreatedDate,
                    Description = p.Description,
                    Status = p.Status.StatusName,
                    Type = p.Type.TypeName,
                    Address = p.Address.AddressName,
                    City = p.City.CityName,
                    Governorate = p.Governorate.GovernorateName,
                    Image = p.PropertyImages.FirstOrDefault().ImageUrl,
                    ShortDescription = p.Description.Length > 40 ? p.Description.Substring(0, 40) + "..." : p.Description

                });

            query = sortOrder switch
            {
                "price_asc" => query.OrderBy(p => p.Price),
                "price_desc" => query.OrderByDescending(p => p.Price),
                "date_asc" => query.OrderBy(p => p.CreatedDate),
                "date_desc" => query.OrderByDescending(p => p.CreatedDate),
                _ => query // Default, no sorting
            };
            return await query.ToListAsync();
        }


        [HttpGet("GetPropertiesForRent")]
        public async Task<IEnumerable<PropertyDTO>> GetPropertiesForRent([FromQuery] string? sortOrder)
        {

            var query = _realEstateContext.TbProperties
                .Where(x => x.CurrentState == true && x.StatusId == 2)
                .Select(p => new PropertyDTO
                {
                    PropertyId = p.PropertyId,
                    Area = p.Area,
                    Bedrooms = p.Bedrooms,
                    Bathrooms = p.Bathrooms,
                    Price = p.Price,
                    Negotiable = p.Negotiable,
                    CreatedDate = p.CreatedDate,
                    Description = p.Description,
                    Status = p.Status.StatusName,
                    Type = p.Type.TypeName,
                    Address = p.Address.AddressName,
                    City = p.City.CityName,
                    Governorate = p.Governorate.GovernorateName,
                    Image = p.PropertyImages.FirstOrDefault().ImageUrl,
                    ShortDescription = p.Description.Length > 40 ? p.Description.Substring(0, 40) + "..." : p.Description

                });

            query = sortOrder switch
            {
                "price_asc" => query.OrderBy(p => p.Price),
                "price_desc" => query.OrderByDescending(p => p.Price),
                "date_asc" => query.OrderBy(p => p.CreatedDate),
                "date_desc" => query.OrderByDescending(p => p.CreatedDate),
                _ => query // Default, no sorting
            };
            return await query.ToListAsync();
        }


        [HttpGet("GetCities")]
        public async Task<IEnumerable<CitiesDTO>> GetCities(int governorateId)
        {
            var cities = await _realEstateContext.TbCities
                                 .Where(c => c.GovernorateId == governorateId)
                                 .Select(p => new CitiesDTO
                                 { 
                                    CityId = p.CityId,
                                    CityName = p.CityName,
                                                                  })
                                 .ToListAsync();
            return cities;
        }



    }
}
