
using System;
using RealEstate.Areas.AdminArea289.ViewModels;

namespace RealEstate.Areas.AdminArea289.Controllers
{
    [Authorize(Roles = "Admin,Data Entry")]
    [Area("AdminArea289")]
	public class PropertiesController : Controller
	{
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
		private readonly UserManager<ApplicationUser> _userManager;
        private readonly RealEstateContext _realEstateContext;
		public PropertiesController(IUnitOfWork unitOfWork, IMapper mapper, UserManager<ApplicationUser> userManager, RealEstateContext realEstateContext)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_userManager = userManager;
			_realEstateContext = realEstateContext;
		}

		[HttpGet]
        public async Task<IActionResult> Index()
        {
            var properties = await _unitOfWork.Properties.GetAllWithNamesAsync();

            var propertyVM = _mapper.Map<IEnumerable<TbProperty>, IEnumerable<PropertyVM>>(properties);

            return View(propertyVM);
        }

        [HttpGet]
        public async Task<IActionResult> DeletedProperties()
        {
            var properties = await _unitOfWork.Properties.GetAllDeletedWithNamesAsync();
            var propertyVM = _mapper.Map<IEnumerable<TbProperty>, IEnumerable<PropertyVM>>(properties);

            return View(propertyVM);
        }


        public async Task<IActionResult> Create()
        {
            var property = new PropertyVM();

            // including select items
            await IcludeSelectItem();

            return View(property);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PropertyVM propertyVM, List<IFormFile> images, IFormFile? ContractImage)
        {

            ModelState.Remove("PropertyId");
            ModelState.Remove("CurrentState");
			ModelState.Remove("CreatedBy");

			if (!ModelState.IsValid)
                return View(propertyVM);

			propertyVM.CreatedBy = _userManager.GetUserId(User);
            propertyVM.CreatedDate = DateTime.UtcNow;

            if (ContractImage != null)
                propertyVM.PropertyContractImage = await Helper.SaveImageAsync(ContractImage, "Properties", 600, 454);



            var property = _mapper.Map<PropertyVM, TbProperty>(propertyVM);

			var propertyRow = await _unitOfWork.Properties.AddAsync(property);


            // Save images associated with the property
            foreach (var image in images)
			{
				if (image != null && image.Length > 0)
				{
					var imageUrl = await Helper.SaveImageAsync(image,"Properties", 850, 650);  // Method to save the image file and get the URL

                    var propertyImage = new TbPropertyImage
					{
						ImageUrl = imageUrl,
						PropertyId = propertyRow.PropertyId // Set the foreign key
					};

					await _unitOfWork.PropertyImages.AddAsync(propertyImage);
				}
			}

			await _unitOfWork.SaveChangesAsync();


			return RedirectToAction(nameof(Index));

        }



		public async Task<IActionResult> Edit(int? id) => await ItemControllerHandler(id, nameof(Edit));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, PropertyVM propertyVM, List<int> DeleteImages, List<IFormFile> images, IFormFile? ContractImage)
        {
            if (id != propertyVM.PropertyId)
            {
                ModelState.AddModelError(string.Empty, "An invalid or missing ID was provided for edit");
                // including select items
                await IcludeSelectItem();

                return View(propertyVM);
            }


            if (ModelState.IsValid)
            {
                try
                {
                    var propertyForImage = await _unitOfWork.Properties.GetAsync(id);
                    if (propertyForImage == null)
                    {
                        ModelState.AddModelError("", "The types does not exist.");
                        return View(propertyVM);
                    }

                    propertyVM.CreatedDate = propertyForImage.CreatedDate;
                    propertyVM.UpdatedBy = _userManager.GetUserId(User);
					propertyVM.UpdatedDate = DateTime.UtcNow;

                    // Convert SoldOrRenteledDate to UTC
                    if (propertyVM.SoldOrRenteledDate.HasValue)
                    {
                        propertyVM.SoldOrRenteledDate = TimeZoneInfo.ConvertTimeToUtc(propertyVM.SoldOrRenteledDate.Value);
                    }

      


                    //contract image
                    if (ContractImage != null)
                    {
                        if (propertyForImage.PropertyContractImage != null)
                        {
                            // Delete Existing logo

                            // Get the full path to the image file
                            string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", propertyForImage.PropertyContractImage.TrimStart('/'));

                            // Delete the image file if it exists
                            if (System.IO.File.Exists(imagePath))
                            {
                                System.IO.File.Delete(imagePath);
                            }
                        }

                        propertyVM.PropertyContractImage = await Helper.SaveingImageAsync(ContractImage, "Properties");
                    }
                    else
                    {
                        propertyVM.PropertyContractImage = propertyForImage.PropertyContractImage;
                    }


                    _unitOfWork.Properties.Detach(propertyForImage);

                    var property = _mapper.Map<PropertyVM, TbProperty>(propertyVM);
					_unitOfWork.Properties.Update(property);
                    await _unitOfWork.SaveChangesAsync();


					// Handle Deletion of Images
					if (DeleteImages != null && DeleteImages.Any())
					{

                        var ThisProperty = await _realEstateContext.TbProperties
                             .Include(p => p.PropertyImages)
                             .FirstOrDefaultAsync(p => p.PropertyId == id);


                        if(ThisProperty != null)
                        {
                            // remove images from path
                            // Get the full path to the image file
                            foreach (var image in ThisProperty.PropertyImages)
                            {
                                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", image.ImageUrl.TrimStart('/'));

                                // Delete the image file if it exists
                                if (System.IO.File.Exists(imagePath))
                                {
                                    System.IO.File.Delete(imagePath);
                                }
                            }




                            var imagesToDelete = ThisProperty.PropertyImages
                                                         .Where(img => DeleteImages.Contains(img.ImageId))
                                                         .ToList();

                            _realEstateContext.TbPropertyImages.RemoveRange(imagesToDelete);
                        }
                    }




					// Handle Addition of New Images
					if (images != null && images.Any())
					{
						foreach (var file in images)
						{
							if (file.Length > 0)
							{
								var imageUrl = await Helper.SaveImageAsync(file,"Properties",850,650); // Method to save the file and return URL
								var newImage = new TbPropertyImage
								{
									ImageUrl = imageUrl,
									PropertyId = property.PropertyId,
									CurrentState = true // Set based on your logic
								};
								await _unitOfWork.PropertyImages.AddAsync(newImage);
							}
						}
					}

					//Save changes to the database

					await _unitOfWork.SaveChangesAsync();

					return RedirectToAction(nameof(Index)); // Or any other view
					
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }


            // including select items
            await IcludeSelectItem();

            return View(propertyVM);
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

                return View("Delete", new PropertyVM());
            }

            var property = await _unitOfWork.Properties.GetWithImagesAsync(id.Value);

            if (property?.CurrentState == false)
                property = null;

            if (property is null)
            {
                ModelState.AddModelError(string.Empty, "The property could not be found.");
                return View("Delete", new PropertyVM());
            }

            try
            {
                // remove images from path
                // Get the full path to the image file
                foreach (var image in property.PropertyImages)
                {
                    string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", image.ImageUrl.TrimStart('/'));

                    // Delete the image file if it exists
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }
                if (!string.IsNullOrEmpty(property.PropertyContractImage))
                {
                    // Get the full path to the image file
                    string imageContractPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", property.PropertyContractImage.TrimStart('/'));

                    // Delete the image file if it exists
                    if (System.IO.File.Exists(imageContractPath))
                    {
                        System.IO.File.Delete(imageContractPath);
                    }
                }

                    


                _unitOfWork.Properties.Delete(property);
                await _unitOfWork.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(property);

        }



        private async Task<IActionResult> ItemControllerHandler(int? id, string viewName)
        {
            if (!id.HasValue)
                return BadRequest();
            var property = await _unitOfWork.Properties.GetWithNamesAsync(id.Value);

            if (property?.CurrentState == false)
                property = null;

            if (property == null)
                return NotFound();

            // including select items
            await IcludeSelectItem();

            var propertyVM = _mapper.Map<TbProperty, PropertyVM>(property);

            var egyptTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");

            propertyVM.SoldOrRenteledDate = propertyVM.SoldOrRenteledDate.HasValue
                    ? TimeZoneInfo.ConvertTimeFromUtc(propertyVM.SoldOrRenteledDate.Value, egyptTimeZone)
                    : (DateTime?)null;


            propertyVM.CreatedDate = TimeZoneInfo.ConvertTimeFromUtc(propertyVM.CreatedDate, egyptTimeZone);



            propertyVM.UpdatedDate = propertyVM.UpdatedDate.HasValue
                                ? TimeZoneInfo.ConvertTimeFromUtc(propertyVM.UpdatedDate.Value, egyptTimeZone)
                                : (DateTime?)null;




            return View(viewName, propertyVM);
        }

        private async Task IcludeSelectItem()
        {
            ViewBag.Governorates = new SelectList(await _unitOfWork.Governorates.GetAllAsync(), "GovernorateId", "GovernorateName");
            ViewBag.Cities = new SelectList(await _unitOfWork.Cities.GetAllAsync(), "CityId", "CityName");
            ViewBag.Owners = new SelectList(await _unitOfWork.Owners.GetAllOwnersAsync(), "OwnerId", "FullName");
            ViewBag.Statuses = new SelectList(await _unitOfWork.Statuses.GetAllAsync(), "StatusId", "StatusName");
            ViewBag.Types = new SelectList(await _unitOfWork.Types.GetAllAsync(), "TypeId", "TypeName");
            ViewBag.Addresses = new SelectList(await _unitOfWork.Addresses.GetAllAsync(), "AddressId", "AddressName");
            ViewBag.Currencies = new SelectList(await _unitOfWork.Currencies.GetAllAsync(), "CurrencyId", "CurrencyName");

        }
    }
}
