using Microsoft.AspNetCore.Mvc.ViewEngines;
using RealEstate.ViewModels;

namespace RealEstate.Controllers
{

    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly RealEstateContext _realEstateContext;
        private readonly ICompositeViewEngine _viewEngine;

        public HomeController(IUnitOfWork unitOfWork, IMapper mapper, RealEstateContext realEstateContext, ICompositeViewEngine viewEngine)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _realEstateContext = realEstateContext;
            _viewEngine = viewEngine;

        }

        [HttpGet]
        public async Task<IActionResult> de()
        {
            MainHomeVM mainHomeVM = new();
            mainHomeVM.FeaturedProperties = _mapper.Map<IEnumerable<TbProperty>, IEnumerable<PropertyVM>>(await _unitOfWork.Properties.FeaturedPropertiesAsync(10));
            mainHomeVM.LatestProperties = _mapper.Map<IEnumerable<TbProperty>, IEnumerable<PropertyVM>>(await _unitOfWork.Properties.LatestPropertiesAsync(6));

            mainHomeVM.Setting = await _realEstateContext.TbSettings.FirstOrDefaultAsync();




            mainHomeVM.NumOfRealySold = await _realEstateContext.TbProperties.CountAsync(a => a.StatusId == 1 && a.IsSoldOrRenteled);
            mainHomeVM.NumOfForSale = await _realEstateContext.TbProperties.CountAsync(a => a.StatusId == 1 && a.IsSoldOrRenteled == false && a.CurrentState);
            mainHomeVM.NumOfRealyRentaled = await _realEstateContext.TbProperties.CountAsync(a => a.StatusId == 2 && a.IsSoldOrRenteled);
            mainHomeVM.NumOfForRental = await _realEstateContext.TbProperties.CountAsync(a => a.StatusId == 2 && a.IsSoldOrRenteled == false && a.CurrentState == true);



            mainHomeVM.PropertyTypes = await _realEstateContext.TbProperties
                .Where(p => p.CurrentState == true)
                .Include(p => p.Type)
                .GroupBy(p => new { p.Type.TypeName, p.Type.TypeImage }) // Group by both TypeName and TypeImage //.GroupBy(p => p.Type.TypeName) 
                .Select(g => new PropertyTypeCount
                {
                    Type = g.Key.TypeName,
                    Count = g.Count(),
                    TypeImage = g.Key.TypeImage // Get the property type image

                })
                .ToListAsync();


            ViewBag.Governorates = new SelectList(await _unitOfWork.Governorates.GetAllAsync(), "GovernorateId", "GovernorateName");
            ViewBag.Types = new SelectList(await _unitOfWork.Types.GetAllAsync(), "TypeId", "TypeName");



            // minimum abaliable sale and rent prices
            mainHomeVM.MinAvaliableRentPrice = await _realEstateContext.TbProperties.Where(a => a.CurrentState == true && a.StatusId == 2).MinAsync(a => a.Price);
            mainHomeVM.MinAvaliableSalePrice = await _realEstateContext.TbProperties.Where(a => a.CurrentState == true && a.StatusId == 1).MinAsync(a => a.Price);



            return View(mainHomeVM);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            MainHomeVM mainHomeVM = new ();
            mainHomeVM.FeaturedProperties = _mapper.Map<IEnumerable<TbProperty>, IEnumerable<PropertyVM>>(await _unitOfWork.Properties.FeaturedPropertiesAsync(10));
            mainHomeVM.LatestProperties = _mapper.Map<IEnumerable<TbProperty>, IEnumerable<PropertyVM>>(await _unitOfWork.Properties.LatestPropertiesAsync(6));

            mainHomeVM.Setting = await _realEstateContext.TbSettings.FirstOrDefaultAsync();




            mainHomeVM.NumOfRealySold = await _realEstateContext.TbProperties.CountAsync(a => a.StatusId == 1 && a.IsSoldOrRenteled);
            mainHomeVM.NumOfForSale = await _realEstateContext.TbProperties.CountAsync(a => a.StatusId == 1 && a.IsSoldOrRenteled == false && a.CurrentState);
            mainHomeVM.NumOfRealyRentaled = await _realEstateContext.TbProperties.CountAsync(a => a.StatusId == 2 && a.IsSoldOrRenteled);
            mainHomeVM.NumOfForRental = await _realEstateContext.TbProperties.CountAsync(a => a.StatusId == 2 && a.IsSoldOrRenteled == false && a.CurrentState == true);



            mainHomeVM.PropertyTypes = await _realEstateContext.TbProperties
                .Where(p => p.CurrentState == true)
                .Include(p => p.Type)
                .GroupBy(p => new { p.Type.TypeName, p.Type.TypeImage }) // Group by both TypeName and TypeImage //.GroupBy(p => p.Type.TypeName) 
                .Select(g => new PropertyTypeCount
                {
                    Type = g.Key.TypeName,    
                    Count = g.Count(),
                    TypeImage = g.Key.TypeImage // Get the property type image

                })
                .ToListAsync();


            ViewBag.Governorates = new SelectList(await _unitOfWork.Governorates.GetAllAsync(), "GovernorateId", "GovernorateName");
            ViewBag.Types = new SelectList(await _unitOfWork.Types.GetAllAsync(), "TypeId", "TypeName");



            // minimum abaliable sale and rent prices
            mainHomeVM.MinAvaliableRentPrice = await _realEstateContext.TbProperties.Where(a=>a.CurrentState == true && a.StatusId == 2).MinAsync(a=>a.Price);
            mainHomeVM.MinAvaliableSalePrice = await _realEstateContext.TbProperties.Where(a => a.CurrentState == true && a.StatusId == 1).MinAsync(a => a.Price);



            return View(mainHomeVM);
        }

        [HttpGet]
        public IActionResult Search(int statusId,int? typeId, int? governorateId, int? cityId, int? bedrooms, int? price)
        {
            var model = new SearchModel()
            {
                statusId = statusId,
                typeId = typeId,
                governorateId = governorateId,
                cityId = cityId,
                bedrooms = bedrooms,
                price = price
            };
            return View(model);
        }

   
        [HttpGet]
        public JsonResult GetCities(int governorateId)
        {
            var cities = _realEstateContext.TbCities
                                 .Where(c => c.GovernorateId == governorateId)
                                 .Select(c => new { c.CityId, c.CityName })
                                 .ToList();
            return Json(cities); 
        }


        public IActionResult PropertiesGrid()
        {
            return View();
        }


        public async Task<IActionResult> PropertyDetails(int? id)
        {
            if (!id.HasValue)
                return BadRequest();

            return View(_mapper.Map<TbProperty, PropertyVM>(await _unitOfWork.Properties.GetWithNamesAsync(id.Value)));

        }

        public IActionResult Map()
        {
            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
