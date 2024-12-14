
namespace RealEstate.Areas.AdminArea289.Controllers
{
    [Authorize(Roles = "Admin,Data Entry")]

    [Area("AdminArea289")]
	public class GovernoratesController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
		
		public GovernoratesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var governorates = await _unitOfWork.Governorates.GetAllAsync();
            var governoratesVM = _mapper.Map<IEnumerable<TbGovernorate>, IEnumerable<GovernorateVM>>(governorates);

            return View(governoratesVM);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GovernorateVM governorateVM)
        {

			ModelState.Remove("GovernorateId");
			ModelState.Remove("CurrentState");


			if (!ModelState.IsValid)
                return View(governorateVM);

            var governorate = _mapper.Map<GovernorateVM, TbGovernorate>(governorateVM);


            await _unitOfWork.Governorates.AddAsync(governorate);
            await _unitOfWork.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }




		public async Task<IActionResult> Edit(int? id) => await ItemControllerHandler(id, nameof(Edit));

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit([FromRoute] int id, GovernorateVM governorateVM)
		{

            if (id != governorateVM.GovernorateId)
            {
                ModelState.AddModelError(string.Empty, "An invalid or missing ID was provided for edit");
                return View(governorateVM);
            }


            if (ModelState.IsValid)
			{
				try
				{
                    var governorate = _mapper.Map<GovernorateVM, TbGovernorate>(governorateVM);

                    _unitOfWork.Governorates.Update(governorate);
					await _unitOfWork.SaveChangesAsync();
					return RedirectToAction(nameof(Index));
				}
				catch (Exception ex)
				{
					ModelState.AddModelError("", ex.Message);
				}
			}

			return View(governorateVM);


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

                return View("Delete", new GovernorateVM());
            }


            var governorate = await _unitOfWork.Governorates.GetAsync(id.Value);
			
			if (governorate?.CurrentState == false)
				governorate = null;

            if (governorate is null)
            {
                ModelState.AddModelError(string.Empty, "The governorate could not be found.");
                return View("Delete", new OwnerVM());
            }

            try
			{
				_unitOfWork.Governorates.Delete(governorate);
				await _unitOfWork.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				ModelState.AddModelError(string.Empty, ex.Message);
			}
			return View(governorate);

		}



		private async Task<IActionResult> ItemControllerHandler(int? id, string viewName)
		{
			if (!id.HasValue)
				return BadRequest();
			var governorate = await _unitOfWork.Governorates.GetAsync(id.Value);

			if (governorate?.CurrentState == false)
				governorate = null;

			if (governorate == null)
				return NotFound();


            var governorateVM = _mapper.Map<TbGovernorate, GovernorateVM>(governorate);


            return View(viewName, governorateVM);
		}


	}
}
