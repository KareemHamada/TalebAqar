using System.Linq;

namespace RealEstate.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly RealEstateContext _context;
        public HomeController(IUnitOfWork unitOfWork, RealEstateContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            HomeVM homeVM = new HomeVM();
            
            // status sold
            homeVM.NumOfTotalSales = await _context.TbProperties.CountAsync(a=>a.StatusId == 1);
            homeVM.NumOfRealySold = await _context.TbProperties.CountAsync(a => a.StatusId == 1 && a.IsSoldOrRenteled);
            homeVM.NumOfForSale = await _context.TbProperties.CountAsync(a => a.StatusId == 1 && a.IsSoldOrRenteled == false && a.CurrentState);

            homeVM.NumOfDeletedSales = await _context.TbProperties.CountAsync(a => a.StatusId == 1 && a.IsSoldOrRenteled == false && a.CurrentState == false);

            // status rentaled
            homeVM.NumOfTotalRentals = await _context.TbProperties.CountAsync(a => a.StatusId == 2);
            homeVM.NumOfRealyRentaled = await _context.TbProperties.CountAsync(a => a.StatusId == 2 && a.IsSoldOrRenteled);
            homeVM.NumOfForRental = await _context.TbProperties.CountAsync(a => a.StatusId == 2 && a.IsSoldOrRenteled == false && a.CurrentState == true);
            homeVM.NumOfDeletedRentals = await _context.TbProperties.CountAsync(a => a.StatusId == 2 && a.IsSoldOrRenteled == false && a.CurrentState == false);


            // sales numbers
            homeVM.TotalPriceSales = await _context.TbProperties.Where(a=>a.StatusId == 1).SumAsync(a => a.Price);


            var HighestPriceSold = await _context.TbProperties.Where(a => a.StatusId == 1 && a.IsSoldOrRenteled).MaxAsync(a => (decimal?)a.Price);
            homeVM.HighestPriceSold = HighestPriceSold ?? 0; // Return 0 if highestPrice is null
            
            var LowestPriceSold = await _context.TbProperties.Where(a => a.StatusId == 1 && a.IsSoldOrRenteled).MinAsync(a => (decimal?)a.Price);
            homeVM.LowestPriceSold = LowestPriceSold ?? 0;


            var HighestPriceAvaliableForSale = await _context.TbProperties.Where(a => a.StatusId == 1 && a.IsSoldOrRenteled == false && a.CurrentState).MaxAsync(a => (decimal?)a.Price);
            homeVM.HighestPriceAvaliableForSale = HighestPriceAvaliableForSale ?? 0;


            var LowestPriceAvaliableForSale = await _context.TbProperties.Where(a => a.StatusId == 1 && a.IsSoldOrRenteled == false && a.CurrentState).MinAsync(a => (decimal?)a.Price);
            homeVM.LowestPriceAvaliableForSale = LowestPriceAvaliableForSale ?? 0;


            // rentaled numbers
            homeVM.TotalPriceRentals = await _context.TbProperties.Where(a => a.StatusId == 2).SumAsync(a => a.Price);

            var HighestPriceRentaled = await _context.TbProperties.Where(a => a.StatusId == 2 && a.IsSoldOrRenteled).MaxAsync(a => (decimal?)a.Price);
            homeVM.HighestPriceRentaled = HighestPriceRentaled ?? 0;

            var LowestPriceRentaled = await _context.TbProperties.Where(a => a.StatusId == 2 && a.IsSoldOrRenteled).MinAsync(a => (decimal?)a.Price);
            homeVM.LowestPriceRentaled = LowestPriceRentaled ?? 0;

            var HighestPriceAvaliableForRent = await _context.TbProperties.Where(a => a.StatusId == 2 && a.IsSoldOrRenteled == false && a.CurrentState).MaxAsync(a => (decimal?)a.Price);
            homeVM.HighestPriceAvaliableForRent = HighestPriceAvaliableForRent ?? 0;


            var LowestPriceAvaliableForRent = await _context.TbProperties.Where(a => a.StatusId == 2 && a.IsSoldOrRenteled == false && a.CurrentState).MinAsync(a => (decimal?)a.Price);
            homeVM.LowestPriceAvaliableForRent = LowestPriceAvaliableForRent ?? 0;

			// governments and cities

			// start of governments
			homeVM.TotalGovernorates = await _context.TbGovernorates.CountAsync();

			var res = await _context.TbProperties
				.Where(p => p.StatusId == 1)
				.GroupBy(p => p.Governorate)// Group by city
				.Select(g => new
				{
					Governorate = g.Key,
					Count = g.Count() // Count sold properties in each city
				})
				.OrderByDescending(g => g.Count) // Order by count descending
				.FirstOrDefaultAsync(); // Get the first city with the highest count


			homeVM.BiggestSellingGovernorate = res?.Governorate?.GovernorateName;
			homeVM.NumOfPropertiesForBiggestSellingGovernorate = res?.Count ?? 0;


			var res2 = await _context.TbProperties
				.Where(p => p.StatusId == 2)
				.GroupBy(p => p.Governorate)// Group by city
				.Select(g => new
				{
					Governorate = g.Key,
					Count = g.Count() // Count sold properties in each city
				})
				.OrderByDescending(g => g.Count) // Order by count descending
				.FirstOrDefaultAsync(); // Get the first city with the highest count


			homeVM.BiggestRentalCity = res2?.Governorate?.GovernorateName;
			homeVM.NumOfPropertiesForBiggestRentalCity = res2?.Count ?? 0;
			// end of governments



            // start of cities
			homeVM.TotalCities = await _context.TbCities.CountAsync();
			var result = await _context.TbProperties
                .Where(p => p.StatusId == 1)
                .GroupBy(p => p.City)// Group by city
                .Select(g => new
                {
                    City = g.Key,
                    Count = g.Count() // Count sold properties in each city
                })
                .OrderByDescending(g => g.Count) // Order by count descending
                .FirstOrDefaultAsync(); // Get the first city with the highest count


            homeVM.BiggestSellingCity = result?.City?.CityName;
            homeVM.NumOfPropertiesForBiggestSellingCity = result?.Count ?? 0;


            var result2 = await _context.TbProperties
                .Where(p => p.StatusId == 2)
                .GroupBy(p => p.City)// Group by city
                .Select(g => new
                {
                    City = g.Key,
                    Count = g.Count() // Count sold properties in each city
                })
                .OrderByDescending(g => g.Count) // Order by count descending
                .FirstOrDefaultAsync(); // Get the first city with the highest count


            homeVM.BiggestRentalCity = result2?.City?.CityName;
            homeVM.NumOfPropertiesForBiggestRentalCity = result2?.Count ?? 0;
			// end of cities


			// owner
			homeVM.NumOfOwners = await _context.TbOwners.CountAsync();


			// profit

            // sales
			homeVM.TotalSalesProfit = await _context.TbProperties.Where(a => a.StatusId == 1).SumAsync(a => a.CostPrice);

			var MaximumSalesProfit = await _context.TbProperties.Where(a => a.StatusId == 1).MaxAsync(a => (decimal?)a.CostPrice);
			homeVM.MaximumSalesProfit = MaximumSalesProfit ?? 0;


			var MinimumSalesProfit = await _context.TbProperties.Where(a => a.StatusId == 1).MinAsync(a => (decimal?)a.CostPrice);
			homeVM.MinimumSalesProfit = MinimumSalesProfit ?? 0;



			// rentals

			homeVM.TotalRentalsProfit = await _context.TbProperties.Where(a => a.StatusId == 2).SumAsync(a => a.CostPrice);

			var MaximumRentalsProfit = await _context.TbProperties.Where(a => a.StatusId == 2).MaxAsync(a => (decimal?)a.CostPrice);
			homeVM.MaximumRentalsProfit = MaximumRentalsProfit ?? 0;


			var MinimumRentalsProfit = await _context.TbProperties.Where(a => a.StatusId == 2).MinAsync(a => (decimal?)a.CostPrice);
			homeVM.MinimumRentalsProfit = MinimumRentalsProfit ?? 0;

			return View(homeVM);
        }
    }
}
