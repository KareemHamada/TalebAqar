
namespace RealEstate.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]

    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {

            var users = await _userManager.Users.Select(u => new UserVM
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                UserName = u.UserName,
                Email = u.Email,
                Roles = _userManager.GetRolesAsync(u).GetAwaiter().GetResult()

            }).ToListAsync();
            return View(users);
        }


        public async Task<IActionResult> Details(string id, string ViewName = nameof(Details))
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                ViewBag.ErrorMessage = "User ID is required.";
                return View("Error");
            }


            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = "User not found.";
                return View("Error");
            }


            var model = new UserVM
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Id = user.Id,
                Roles = await _userManager.GetRolesAsync(user)
            };

            return View(ViewName, model);
        }


        public async Task<IActionResult> Edit(string id) => await Details(id, nameof(Edit));

        [HttpPost]
        public async Task<IActionResult> Edit(string id, UserVM model)
        {
            if (id != model.Id)
            {
                ViewBag.ErrorMessage = "User ID is required.";
                return View("Error");
            }
            if (!ModelState.IsValid)
                return View(model);


            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    ViewBag.ErrorMessage = "User not found.";
                    return View("Error");
                }

                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.UserName = model.UserName;

                await _userManager.UpdateAsync(user);

                return RedirectToAction(nameof(Index));


            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(model);
        }


        public async Task<IActionResult> Delete(string id) => await Details(id, nameof(Delete));


        [ActionName("Delete")]
        [HttpPost]
        public async Task<IActionResult> ConfirmDelete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                ViewBag.ErrorMessage = "User ID is required.";
                return View("Error");
            }

            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user is null)
                {
                    ViewBag.ErrorMessage = "User not found.";
                    return View("Error");
                }

                await _userManager.DeleteAsync(user);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View();

        }
    }
}
