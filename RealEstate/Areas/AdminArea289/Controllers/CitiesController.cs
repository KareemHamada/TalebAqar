namespace RealEstate.Areas.AdminArea289.Controllers
{


    [Authorize(Roles = "Admin,Data Entry")]
    [Area("AdminArea289")]
	public class CitiesController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public CitiesController(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;

		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			var Cities = await _unitOfWork.Cities.GetAllWithGovernorateAsync();
			var cityVM = _mapper.Map<IEnumerable<TbCity>, IEnumerable<CityVM>>(Cities);

			return View(cityVM);
		}

		public async Task<IActionResult> Create()
		{
			var Governorates = await _unitOfWork.Governorates.GetAllAsync();
			SelectList listItems = new SelectList(Governorates, "GovernorateId", "GovernorateName");
			ViewBag.Governorates = listItems;

			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CityVM cityVM)
		{

			ModelState.Remove("CityId");
			ModelState.Remove("CurrentState");

			if (!ModelState.IsValid)
				return View(cityVM);

			var city = _mapper.Map<CityVM, TbCity>(cityVM);

			await _unitOfWork.Cities.AddAsync(city);
			await _unitOfWork.SaveChangesAsync();

			return RedirectToAction(nameof(Index));

		}


		public async Task<IActionResult> Edit(int? id) => await ItemControllerHandler(id, nameof(Edit));

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit([FromRoute] int id, CityVM cityVM)
		{
			if (id != cityVM.CityId)
			{
				ModelState.AddModelError(string.Empty, "An invalid or missing ID was provided for edit") ;

				var governorates = await _unitOfWork.Governorates.GetAllAsync();
				ViewBag.Governorates = new SelectList(governorates, "GovernorateId", "GovernorateName");

				return View(cityVM);

			}


			if (ModelState.IsValid)
			{
				try
				{
					var city = _mapper.Map<CityVM, TbCity>(cityVM);


					_unitOfWork.Cities.Update(city);
					await _unitOfWork.SaveChangesAsync();
					return RedirectToAction(nameof(Index));
				}
				catch (Exception ex)
				{
					ModelState.AddModelError("", ex.Message);
				}
			}
			var governoratesOnFailure = await _unitOfWork.Governorates.GetAllAsync();
			ViewBag.Governorates = new SelectList(governoratesOnFailure, "GovernorateId", "GovernorateName");

			return View(cityVM);

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

				return View("Delete", new CityVM());
			}
				

			var city = await _unitOfWork.Cities.GetAsync(id.Value);

			if (city?.CurrentState == false)
				city = null;

			if (city is null)
			{
				ModelState.AddModelError(string.Empty, "The city could not be found.");
				return View("Delete", new CityVM());
			}

			try
			{
				_unitOfWork.Cities.Delete(city);
				await _unitOfWork.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				ModelState.AddModelError(string.Empty, ex.Message);
			}
			return View(city);

		}



		private async Task<IActionResult> ItemControllerHandler(int? id, string viewName)
		{
			if (!id.HasValue)
				return BadRequest();
			var city = await _unitOfWork.Cities.GetAsync(id.Value);

			if (city?.CurrentState == false)
				city = null;

			if (city == null)
				return NotFound();

			 var Governorates = await _unitOfWork.Governorates.GetAllAsync();
			SelectList listItems = new SelectList(Governorates, "GovernorateId", "GovernorateName");
			ViewBag.Governorates = listItems;


			var cityVM = _mapper.Map<TbCity, CityVM>(city);

			return View(viewName, cityVM);
		}
	}
}
