using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> Index()
        {
            MainHomeVM mainHomeVM = new ();
            mainHomeVM.FeaturedProperties = _mapper.Map<IEnumerable<TbProperty>, IEnumerable<PropertyVM>>(await _unitOfWork.Properties.FeaturedPropertiesAsync(10));

            mainHomeVM.LatestProperties = _mapper.Map<IEnumerable<TbProperty>, IEnumerable<PropertyVM>>(await _unitOfWork.Properties.LatestPropertiesAsync(6));

            //mainHomeVM.Setting = await _realEstateContext.TbSettings.FirstOrDefaultAsync();


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



            // minimum and maximum abaliable sale and rent prices
            if (_realEstateContext.TbProperties.Any())
            {
                mainHomeVM.MinAvaliableRentPrice = await _realEstateContext.TbProperties.Where(a => a.CurrentState == true && a.StatusId == 2).MinAsync(a => a.Price);

                mainHomeVM.MaxAvaliableRentPrice = await _realEstateContext.TbProperties.Where(a => a.CurrentState == true && a.StatusId == 2).MaxAsync(a => a.Price);


                mainHomeVM.MinAvaliableSalePrice = await _realEstateContext.TbProperties.Where(a => a.CurrentState == true && a.StatusId == 1).MinAsync(a => a.Price);
                mainHomeVM.MaxAvaliableSalePrice = await _realEstateContext.TbProperties.Where(a => a.CurrentState == true && a.StatusId == 1).MaxAsync(a => a.Price);

            }
           



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

        public IActionResult PropertiesGridForSale()
        {
            return View();
        }

        public IActionResult PropertiesGridForRent()
        {
            return View();
        }
        public IActionResult Wishlist()
        {
            return View();
        }

        public async Task<IActionResult> PropertyDetails(int? id)
        {
            if (!id.HasValue)
                return BadRequest();

            PropertyDetailsVM vm = new();

            var property = await _unitOfWork.Properties.GetAsync(id.Value);
            if(property == null)
                return NotFound();

            property.NumOfViews += 1;

            _unitOfWork.Properties.Update(property);
            await _unitOfWork.SaveChangesAsync();


            vm.propertyVM = _mapper.Map<TbProperty, PropertyVM>(await _unitOfWork.Properties.GetWithNamesAsync(id.Value));
            vm.PropertiesInTheSameGovernorate = _mapper.Map<IEnumerable<TbProperty>, IEnumerable<PropertyVM>>(await _unitOfWork.Properties.PropertiesInTheSameGovernorate(vm.propertyVM.GovernorateId));
            return View(vm);

        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
