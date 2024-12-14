
namespace RealEstate.Areas.AdminArea289.Controllers
{
    [Authorize(Roles = "Admin,Data Entry")]
    [Area("AdminArea289")]
	public class AddressesController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public AddressesController(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;

		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			var Address = await _unitOfWork.Addresses.GetAllWithCityAsync();
			
			var addressVM = _mapper.Map<IEnumerable<TbAddress>, IEnumerable<AddressVM>>(Address);

			return View(addressVM);
		}

		public async Task<IActionResult> Create()
		{
			var Cities = await _unitOfWork.Cities.GetAllAsync();
			SelectList listItems = new SelectList(Cities, "CityId", "CityName");
			ViewBag.Cities = listItems;

			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(AddressVM addressVM)
		{

			ModelState.Remove("AddressId");
			ModelState.Remove("CurrentState");

			if (!ModelState.IsValid)
				return View(addressVM);

			var address = _mapper.Map<AddressVM, TbAddress>(addressVM);

			await _unitOfWork.Addresses.AddAsync(address);
			await _unitOfWork.SaveChangesAsync();

			return RedirectToAction(nameof(Index));

		}


		public async Task<IActionResult> Edit(int? id) => await ItemControllerHandler(id, nameof(Edit));

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit([FromRoute] int id, AddressVM addressVM)
		{
			if (id != addressVM.AddressId)
			{
				ModelState.AddModelError(string.Empty, "An invalid or missing ID was provided for edit");

				var Cities = await _unitOfWork.Cities.GetAllAsync();
				ViewBag.Cities = new SelectList(Cities, "CityId", "CityName");

				return View(addressVM);

			}


			if (ModelState.IsValid)
			{
				try
				{
					var address = _mapper.Map<AddressVM, TbAddress>(addressVM);


					_unitOfWork.Addresses.Update(address);
					await _unitOfWork.SaveChangesAsync();
					return RedirectToAction(nameof(Index));
				}
				catch (Exception ex)
				{
					ModelState.AddModelError("", ex.Message);
				}
			}
			var CitiesOnFailure = await _unitOfWork.Cities.GetAllAsync();
			ViewBag.Cities = new SelectList(CitiesOnFailure, "CityId", "CityName");

			return View(addressVM);

		}

		public async Task<IActionResult> Details(int? id) => await ItemControllerHandler(id, nameof(Details));




		public async Task<IActionResult> Delete(int? id) => await ItemControllerHandler(id, nameof(Delete));


		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ConfirmDelete(int? id)
		{
			//if (!id.HasValue)
			//	return BadRequest();

			if (!id.HasValue)
			{
				ModelState.AddModelError(string.Empty, "An invalid or missing ID was provided for deletion");

				return View("Delete", new AddressVM());
			}


			var address = await _unitOfWork.Addresses.GetAsync(id.Value);

			if (address?.CurrentState == false)
				address = null;

			if (address is null)
			{
				ModelState.AddModelError(string.Empty, "The address could not be found.");
				return View("Delete", new AddressVM());
			}

			try
			{
				_unitOfWork.Addresses.Delete(address);
				await _unitOfWork.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				ModelState.AddModelError(string.Empty, ex.Message);
			}
			return View(address);

		}




		private async Task<IActionResult> ItemControllerHandler(int? id, string viewName)
		{
			if (!id.HasValue)
				return BadRequest();
			var address = await _unitOfWork.Addresses.GetAsync(id.Value);

			if (address?.CurrentState == false)
				address = null;

			if (address == null)
				return NotFound();

			var Cities = await _unitOfWork.Cities.GetAllAsync();
			SelectList listItems = new SelectList(Cities, "CityId", "CityName");
			ViewBag.Cities = listItems;


			var addressVM = _mapper.Map<TbAddress, AddressVM>(address);

			return View(viewName, addressVM);
		}
	}
}
