namespace RealEstate.Areas.Admin.Controllers
{
	[Area("Admin")]
	//[Authorize(Roles = "Admin")]
	public class RolesController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		public RolesController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
		{
			_userManager = userManager;
			_roleManager = roleManager;
		}

		public async Task<IActionResult> Index()
		{

			var roles = await _roleManager.Roles.Select(u => new RoleVM
			{
				Id = u.Id,
				Name = u.Name
			}).ToListAsync();
			return View(roles);
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(RoleVM roleVM)
		{


			if (!ModelState.IsValid)
				return View(roleVM);


			var role = new IdentityRole
			{
				Name = roleVM.Name,
			};

			var result = await _roleManager.CreateAsync(role);

			if (result.Succeeded)
				return RedirectToAction(nameof(Index));

			foreach (var error in result.Errors)
				ModelState.AddModelError(string.Empty, error.Description);


			return View(roleVM);
		}
		public async Task<IActionResult> Details(string id, string ViewName = nameof(Details))
		{
			if (string.IsNullOrWhiteSpace(id))
			{
				ViewBag.ErrorMessage = "User ID is required.";
				return View("Error");
			}


			var role = await _roleManager.FindByIdAsync(id);
			if (role == null)
			{
				ViewBag.ErrorMessage = "User not found.";
				return View("Error");
			}


			var model = new RoleVM
			{
				Id = id,
				Name = role.Name

			};

			return View(ViewName, model);
		}


		public async Task<IActionResult> Edit(string id) => await Details(id, nameof(Edit));

		[HttpPost]
		public async Task<IActionResult> Edit(string id, RoleVM model)
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
				var role = await _roleManager.FindByIdAsync(id);
				if (role == null)
				{
					ViewBag.ErrorMessage = "User not found.";
					return View("Error");
				}

				role.Name = model.Name;


				await _roleManager.UpdateAsync(role);

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
				var role = await _roleManager.FindByIdAsync(id);
				if (role is null)
				{
					ViewBag.ErrorMessage = "User not found.";
					return View("Error");
				}

				await _roleManager.DeleteAsync(role);
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				ModelState.AddModelError(string.Empty, ex.Message);
			}

			return View();

		}

		//
		public async Task<IActionResult> AddOrRemoveUsers(string roleId)
		{
			var role = await _roleManager.FindByIdAsync(roleId);
			if (role is null)
				return NotFound();

			ViewBag.RoleId = roleId;


			var users = await _userManager.Users.ToListAsync();
			var usersInRole = new List<UserInRoleVM>();

			foreach (var user in users)
			{
				var userInRole = new UserInRoleVM()
				{
					UserId = user.Id,
					UserName = user.UserName,
					IsInRole = await _userManager.IsInRoleAsync(user, role.Name)
				};
				usersInRole.Add(userInRole);

			}

			return View(usersInRole);
		}


		[HttpPost]
		public async Task<IActionResult> AddOrRemoveUsers(string roleId,List<UserInRoleVM> users)
		{
			var role = await _roleManager.FindByIdAsync(roleId);
			if (role is null)
				return NotFound();

			if (ModelState.IsValid)
			{
				foreach(var user in users)
				{
					var appUser = await _userManager.FindByIdAsync(user.UserId);
					if (appUser is null)
						return NotFound();

					if (user.IsInRole && !await _userManager.IsInRoleAsync(appUser, role.Name))
						await _userManager.AddToRoleAsync(appUser,role.Name);
					if (!user.IsInRole && await _userManager.IsInRoleAsync(appUser, role.Name))
						await _userManager.RemoveFromRoleAsync(appUser, role.Name);
				}

				return RedirectToAction(nameof(Edit), new { id = roleId });
			}
			return View(users);
		}


	}
}

