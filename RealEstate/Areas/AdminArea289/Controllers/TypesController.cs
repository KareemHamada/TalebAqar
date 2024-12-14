

namespace RealEstate.Areas.AdminArea289.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("AdminArea289")]
	public class TypesController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

        public TypesController(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;

		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			var types = await _unitOfWork.Types.GetAllAsync();

			var typesVM = _mapper.Map<IEnumerable<TbType>, IEnumerable<TypeVM>>(types);

			return View(typesVM);
		}

		public IActionResult Create()
		{
			var type = new TypeVM();
			return View(type);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(TypeVM typeVM, IFormFile? TypeImage)
		{

			ModelState.Remove("TypeId");
			ModelState.Remove("CurrentState");

			if (!ModelState.IsValid)
				return View(typeVM);

			if(TypeImage != null)
				typeVM.TypeImage = await Helper.SaveImageAsync(TypeImage, "Types", 600, 454);


			var type = _mapper.Map<TypeVM, TbType>(typeVM);

			await _unitOfWork.Types.AddAsync(type);
			await _unitOfWork.SaveChangesAsync();

			return RedirectToAction(nameof(Index));

		}


		public async Task<IActionResult> Edit(int? id) => await ItemControllerHandler(id, nameof(Edit));

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit([FromRoute] int id, TypeVM typeVM, IFormFile? TypeImage)
        {
			if (id != typeVM.TypeId)
			{
				ModelState.AddModelError(string.Empty, "An invalid or missing ID was provided for edit");

				return View(typeVM);

			}


			if (ModelState.IsValid)
            {
                try
                {
                    var typeForImage = await _unitOfWork.Types.GetAsync(id);
                    if (typeForImage == null)
                    {
                        ModelState.AddModelError("", "The setting does not exist.");
                        return View(typeVM);
                    }
                    typeVM.TypeImage = TypeImage != null ? await Helper.SaveImageAsync(TypeImage, "Types", 600, 454) : typeForImage.TypeImage;

					_unitOfWork.Types.Detach(typeForImage);

                    var type = _mapper.Map<TypeVM, TbType>(typeVM);


					_unitOfWork.Types.Update(type);
					await _unitOfWork.SaveChangesAsync();
					return RedirectToAction(nameof(Index));
				}
				catch (Exception ex)
				{
					ModelState.AddModelError("", ex.Message);
				}
			}


			return View(typeVM);

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

				return View("Delete", new TypeVM());
			}


			var type = await _unitOfWork.Types.GetAsync(id.Value);

			if (type?.CurrentState == false)
				type = null;

			if (type is null)
			{
				ModelState.AddModelError(string.Empty, "The type could not be found.");
				return View("Delete", new TypeVM());
			}

			try
			{
				_unitOfWork.Types.Delete(type);
				await _unitOfWork.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				ModelState.AddModelError(string.Empty, ex.Message);
			}
			return View(type);

		}




		private async Task<IActionResult> ItemControllerHandler(int? id, string viewName)
		{
			if (!id.HasValue)
				return BadRequest();
			var type = await _unitOfWork.Types.GetAsync(id.Value);

			if (type?.CurrentState == false)
				type = null;

			if (type == null)
				return NotFound();




			var typeVM = _mapper.Map<TbType, TypeVM>(type);

			return View(viewName, typeVM);
		}
	}
}
