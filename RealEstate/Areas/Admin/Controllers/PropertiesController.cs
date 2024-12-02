using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Channels;

namespace RealEstate.Areas.Admin.Controllers
{ 
	[Area("Admin")]
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

        public async Task<IActionResult> Create()
        {
            // including select items
            await IcludeSelectItem();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PropertyVM propertyVM, List<IFormFile> images)
        {

            ModelState.Remove("PropertyId");
            ModelState.Remove("CurrentState");
			ModelState.Remove("CreatedBy");

			if (!ModelState.IsValid)
                return View(propertyVM);

			propertyVM.CreatedBy = _userManager.GetUserId(User);
            propertyVM.CreatedDate = DateTime.Now;
             
			var property = _mapper.Map<PropertyVM, TbProperty>(propertyVM);

			//var propertyRow =  await _unitOfWork.Properties.AddAsync(property);
			//await _unitOfWork.SaveChangesAsync();


			var propertyRow = await _unitOfWork.Properties.AddAsync(property);


			// Save images associated with the property
			foreach (var image in images)
			{
				if (image != null && image.Length > 0)
				{
					var imageUrl = await SaveImageAsync(image,"Properties"); // Method to save the image file and get the URL

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

		// Method to save image to the file system and return the image URL
		//private async Task<string> SaveImageToFileSystem(IFormFile imageFile, string folderName)
		//{
		//	// Implement your logic to save the image file to a specific location
		//	// and return the URL or path of the saved image

		//	string imageUrl = Guid.NewGuid().ToString() + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + imageFile.FileName;
		//	var filePaths = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Uploads/" + folderName, imageUrl);



		//	//string imageUrl = "your/image/path/" + imageFile.FileName;

		//	using (var stream = new FileStream(filePaths, FileMode.Create))
		//	{
		//		await imageFile.CopyToAsync(stream);
		//	}

		//	return filePaths; // Return the URL or relative path
		//}

		private async Task<string> SaveImageAsync(IFormFile file, string folderName)
		{
			var fileName = Path.GetFileNameWithoutExtension(file.FileName);
			var extension = Path.GetExtension(file.FileName);
			var filePath = Path.Combine($"wwwroot/Uploads/{folderName}", $"{fileName}_{Guid.NewGuid()}{extension}");

			using (var stream = new FileStream(filePath, FileMode.Create))
			{
				await file.CopyToAsync(stream);
			}

			// Return the URL to store in the database
			return $"/Uploads/{folderName}/{Path.GetFileName(filePath)}";
		}

		public async Task<IActionResult> Edit(int? id) => await ItemControllerHandler(id, nameof(Edit));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, PropertyVM propertyVM, List<int> DeleteImages, List<IFormFile> images)
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
					propertyVM.UpdatedBy = _userManager.GetUserId(User);
					propertyVM.UpdatedDate = DateTime.Now;

					var property = _mapper.Map<PropertyVM, TbProperty>(propertyVM);
					_unitOfWork.Properties.Update(property);
                    await _unitOfWork.SaveChangesAsync();


					// Handle Deletion of Images
					if (DeleteImages != null && DeleteImages.Any())
					{
						var ThisProperty = await _realEstateContext.TbProperties
							 .Include(p => p.PropertyImages)
							 .FirstOrDefaultAsync(p => p.PropertyId == id);

                        var imagesToDelete = ThisProperty.PropertyImages
													 .Where(img => DeleteImages.Contains(img.ImageId))
													 .ToList();

						_realEstateContext.TbPropertyImages.RemoveRange(imagesToDelete);
					}




					// Handle Addition of New Images
					if (images != null && images.Any())
					{
						foreach (var file in images)
						{
							if (file.Length > 0)
							{
								var imageUrl = await SaveImageAsync(file,"Properties"); // Method to save the file and return URL
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
            //if (!id.HasValue)
            //	return BadRequest();

            if (!id.HasValue)
            {
                ModelState.AddModelError(string.Empty, "An invalid or missing ID was provided for deletion");

                return View("Delete", new PropertyVM());
            }

            var property = await _unitOfWork.Properties.GetAsync(id.Value);

            if (property?.CurrentState == false)
                property = null;

            if (property is null)
            {
                ModelState.AddModelError(string.Empty, "The property could not be found.");
                return View("Delete", new PropertyVM());
            }

            try
            {
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

            return View(viewName, propertyVM);
        }

        private async Task IcludeSelectItem()
        {
            ViewBag.Governorates = new SelectList(await _unitOfWork.Governorates.GetAllAsync(), "GovernorateId", "GovernorateName");
            ViewBag.Cities = new SelectList(await _unitOfWork.Cities.GetAllAsync(), "CityId", "CityName");
            ViewBag.Owners = new SelectList(await _unitOfWork.Owners.GetAllAsync(), "OwnerId", "FullName");
            ViewBag.Statuses = new SelectList(await _unitOfWork.Statuses.GetAllAsync(), "StatusId", "StatusName");
            ViewBag.Types = new SelectList(await _unitOfWork.Types.GetAllAsync(), "TypeId", "TypeName");
            ViewBag.Addresses = new SelectList(await _unitOfWork.Addresses.GetAllAsync(), "AddressId", "AddressName");
        }
    }
}
