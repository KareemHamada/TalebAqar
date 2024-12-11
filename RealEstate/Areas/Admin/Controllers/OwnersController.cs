

namespace RealEstate.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,Data Entry")]
    [Area("Admin")]
	public class OwnersController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public OwnersController(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;

		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			var owners = await _unitOfWork.Owners.GetAllAsync();

			var ownersVM = _mapper.Map<IEnumerable<TbOwner>, IEnumerable<OwnerVM>>(owners);

			return View(ownersVM);
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(OwnerVM ownerVM)
		{

			ModelState.Remove("OwnerId");
			ModelState.Remove("CurrentState");

			if (!ModelState.IsValid)
				return View(ownerVM);

			var owner = _mapper.Map<OwnerVM, TbOwner>(ownerVM);

			await _unitOfWork.Owners.AddAsync(owner);
			await _unitOfWork.SaveChangesAsync();

			return RedirectToAction(nameof(Index));

		}


		public async Task<IActionResult> Edit(int? id) => await ItemControllerHandler(id, nameof(Edit));

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit([FromRoute] int id, OwnerVM ownerVM)
		{
			if (id != ownerVM.OwnerId)
			{
				ModelState.AddModelError(string.Empty, "An invalid or missing ID was provided for edit");

				return View(ownerVM);

			}


			if (ModelState.IsValid)
			{
				try
				{
					var owner = _mapper.Map<OwnerVM, TbOwner>(ownerVM);


					_unitOfWork.Owners.Update(owner);
					await _unitOfWork.SaveChangesAsync();
					return RedirectToAction(nameof(Index));
				}
				catch (Exception ex)
				{
					ModelState.AddModelError("", ex.Message);
				}
			}


			return View(ownerVM);

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

				return View("Delete", new OwnerVM());
			}


			var owner = await _unitOfWork.Owners.GetAsync(id.Value);

			if (owner?.CurrentState == false)
				owner = null;

			if (owner is null)
			{
				ModelState.AddModelError(string.Empty, "The owner could not be found.");
				return View("Delete", new OwnerVM());
			}

			try
			{
				_unitOfWork.Owners.Delete(owner);
				await _unitOfWork.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				ModelState.AddModelError(string.Empty, ex.Message);
			}
			return View(owner);

		}




		private async Task<IActionResult> ItemControllerHandler(int? id, string viewName)
		{
			if (!id.HasValue)
				return BadRequest();
			var owner = await _unitOfWork.Owners.GetAsync(id.Value);

			if (owner?.CurrentState == false)
				owner = null;

			if (owner == null)
				return NotFound();


			var ownerVM = _mapper.Map<TbOwner, OwnerVM>(owner);

			return View(viewName, ownerVM);
		}
	}
}
