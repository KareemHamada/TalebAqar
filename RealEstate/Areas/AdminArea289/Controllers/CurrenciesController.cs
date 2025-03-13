namespace RealEstate.Areas.AdminArea289.Controllers
{
    [Authorize(Roles = "Admin")]

    [Area("AdminArea289")]
    public class CurrenciesController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CurrenciesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var Currencies = await _unitOfWork.Currencies.GetAllAsync();
            var currencyVM = _mapper.Map<IEnumerable<TbCurrency>, IEnumerable<CurrencyVM>>(Currencies);

            return View(currencyVM);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CurrencyVM currencyVM)
        {

            ModelState.Remove("CurrencyId");
            ModelState.Remove("CurrentState");


            if (!ModelState.IsValid)
                return View(currencyVM);

            var currency = _mapper.Map<CurrencyVM, TbCurrency>(currencyVM);


            await _unitOfWork.Currencies.AddAsync(currency);
            await _unitOfWork.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }




        public async Task<IActionResult> Edit(int? id) => await ItemControllerHandler(id, nameof(Edit));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, CurrencyVM currencyVM)
        {

            if (id != currencyVM.CurrencyId)
            {
                ModelState.AddModelError(string.Empty, "An invalid or missing ID was provided for edit");
                return View(currencyVM);
            }


            if (ModelState.IsValid)
            {
                try
                {
                    var currency = _mapper.Map<CurrencyVM, TbCurrency>(currencyVM);

                    _unitOfWork.Currencies.Update(currency);
                    await _unitOfWork.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            return View(currencyVM);


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

                return View("Delete", new CurrencyVM());
            }


            var currency = await _unitOfWork.Currencies.GetAsync(id.Value);

            if (currency?.CurrentState == false)
                currency = null;

            if (currency is null)
            {
                ModelState.AddModelError(string.Empty, "The currency could not be found.");
                return View("Delete", new CurrencyVM());
            }

            try
            {
                _unitOfWork.Currencies.Delete(currency);
                await _unitOfWork.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(currency);

        }



        private async Task<IActionResult> ItemControllerHandler(int? id, string viewName)
        {
            if (!id.HasValue)
                return BadRequest();
            var currency = await _unitOfWork.Currencies.GetAsync(id.Value);

            if (currency?.CurrentState == false)
                currency = null;

            if (currency == null)
                return NotFound();


            var currencyVM = _mapper.Map<TbCurrency, CurrencyVM>(currency);


            return View(viewName, currencyVM);
        }


    }

}
