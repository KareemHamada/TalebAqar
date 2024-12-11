
namespace RealEstate.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
	public class StatusesController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public StatusesController(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;

		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			var statuses = await _unitOfWork.Statuses.GetAllAsync();

			var statusesVM = _mapper.Map<IEnumerable<TbStatus>, IEnumerable<StatusVM>>(statuses);

			return View(statusesVM);
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(StatusVM statusVM)
		{

			ModelState.Remove("StatusId");
			ModelState.Remove("CurrentState");

			if (!ModelState.IsValid)
				return View(statusVM);

			var status = _mapper.Map<StatusVM, TbStatus>(statusVM);

			await _unitOfWork.Statuses.AddAsync(status);
			await _unitOfWork.SaveChangesAsync();

			return RedirectToAction(nameof(Index));

		}


		public async Task<IActionResult> Edit(int? id) => await ItemControllerHandler(id, nameof(Edit));

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit([FromRoute] int id, StatusVM statusVM)
		{
			if (id != statusVM.StatusId)
			{
				ModelState.AddModelError(string.Empty, "An invalid or missing ID was provided for edit");
			
				return View(statusVM);

			}


			if (ModelState.IsValid)
			{
				try
				{
					var status = _mapper.Map<StatusVM, TbStatus>(statusVM);


					_unitOfWork.Statuses.Update(status);
					await _unitOfWork.SaveChangesAsync();
					return RedirectToAction(nameof(Index));
				}
				catch (Exception ex)
				{
					ModelState.AddModelError("", ex.Message);
				}
			}
			

			return View(statusVM);

		}

		public async Task<IActionResult> Details(int? id) => await ItemControllerHandler(id, nameof(Details));




		public async Task<IActionResult> Delete(int? id) => await ItemControllerHandler(id, nameof(Delete));


		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ConfirmDelete(int? id)
		{
			if (!id.HasValue)
			{
				ModelState.AddModelError(string.Empty, "An invalid or missing ID was provided for deletion");

				return View("Delete", new StatusVM());
			}


			var status = await _unitOfWork.Statuses.GetAsync(id.Value);

			if (status?.CurrentState == false)
				status = null;

			if (status is null)
			{
				ModelState.AddModelError(string.Empty, "The status could not be found.");
				return View("Delete", new StatusVM());
			}

			try
			{
				_unitOfWork.Statuses.Delete(status);
				await _unitOfWork.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				ModelState.AddModelError(string.Empty, ex.Message);
			}
			return View(status);

		}




		private async Task<IActionResult> ItemControllerHandler(int? id, string viewName)
		{
			if (!id.HasValue)
				return BadRequest();
			var status = await _unitOfWork.Statuses.GetAsync(id.Value);

			if (status?.CurrentState == false)
				status = null;

			if (status == null)
				return NotFound();

			


			var statusVM = _mapper.Map<TbStatus, StatusVM>(status);

			return View(viewName, statusVM);
		}
	}
}
