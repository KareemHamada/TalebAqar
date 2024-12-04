using DAL.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace RealEstate.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class SettingsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RealEstateContext _realEstateContext;

        public SettingsController(IUnitOfWork unitOfWork, IMapper mapper, UserManager<ApplicationUser> userManager, RealEstateContext realEstateContext)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper; 
            _userManager = userManager;
            _realEstateContext = realEstateContext;
        }

        [HttpGet]
        public async Task<IActionResult> Details()
        {
            var settings = await _realEstateContext.TbSettings.FirstOrDefaultAsync();

            if (settings == null)
                return NotFound();
            var settingVM = _mapper.Map<TbSetting, SettingVM>(settings);

            return View(settingVM);
        }


        public async Task<IActionResult> Edit()
        {
           
            var settings = await _realEstateContext.TbSettings.FirstOrDefaultAsync();


            if (settings == null)
                return NotFound();


            var settingVM = _mapper.Map<TbSetting, SettingVM>(settings);

            return View(settingVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SettingVM settingVM, IFormFile? Logo, IFormFile? MainPanner, IFormFile? PropertyDetailsPanner)
        {

            if (ModelState.IsValid)
            {
                try
                {

                    // Retrieve the existing entity from the database
                    var existingSetting = await _realEstateContext.TbSettings.FirstOrDefaultAsync();
                    if (existingSetting == null)
                    {
                        ModelState.AddModelError("", "The setting does not exist.");
                        return View(settingVM);
                    }

                    settingVM.UpdatedBy = _userManager.GetUserId(User);
                    settingVM.UpdatedDate = DateTime.Now;



                    settingVM.Logo = Logo != null ? await Helper.SaveImageAsync(Logo, "Settings"): existingSetting.Logo;

                    settingVM.MainPanner = MainPanner != null ? await Helper.SaveImageAsync(MainPanner, "Settings") : existingSetting.MainPanner;

                    settingVM.PropertyDetailsPanner = PropertyDetailsPanner != null ? await Helper.SaveImageAsync(PropertyDetailsPanner, "Settings") : existingSetting.PropertyDetailsPanner;

                    // Detach the existing entity
                    _realEstateContext.Entry(existingSetting).State = EntityState.Detached;

                    var setting = _mapper.Map<SettingVM, TbSetting>(settingVM);
                    _unitOfWork.Settings.Update(setting);
                    await _unitOfWork.SaveChangesAsync();
                
                    return RedirectToAction(nameof(Details)); // Or any other view

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View();
        }


    }
}
