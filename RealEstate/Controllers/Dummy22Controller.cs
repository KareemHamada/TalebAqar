//public async Task<IActionResult> PropertiesGrid(string sortOrder, int page = 1, int pageSize = 10)
//{
//    var properties = _realEstateContext.TbProperties.Where(x => x.CurrentState == true).Include(p => p.Address)
//       .Include(p => p.City)
//       .Include(p => p.Governorate)
//       .Include(p => p.Owner)
//       .Include(p => p.Status)
//       .Include(p => p.PropertyImages)
//       .Include(p => p.Type)
//        .AsQueryable();


//    switch (sortOrder)
//    {
//        case "price_asc":
//            properties = properties.OrderBy(p => p.Price);
//            break;
//        case "price_desc":
//            properties = properties.OrderByDescending(p => p.Price);
//            break;
//        case "date_asc":
//            properties = properties.OrderBy(p => p.CreatedDate);
//            break;
//        case "date_desc":
//            properties = properties.OrderByDescending(p => p.CreatedDate);
//            break;
//        default:
//            properties = properties.OrderBy(p => p.CreatedDate); // default sorting
//            break;
//    }

//    int totalItems = await properties.CountAsync();
//    var propertiesList = await properties
//        .Skip((page - 1) * pageSize)
//        .Take(pageSize)
//        .ToListAsync();

//    var viewModel = new PropertiesGridVM
//    {
//        Properties = propertiesList,
//        PageNumber = page,
//        PageSize = pageSize,
//        TotalItems = totalItems
//    };

//    // Check for AJAX request
//    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
//    {
//        var partial1Html = await RenderPartialViewToString("PartialViews/_PropertyList", viewModel);
//        var partial2Html = await RenderPartialViewToString("PartialViews/_PropertyList2", viewModel);

//        // Return JSON containing both partials
//        return Json(new { partial1Html, partial2Html });

//        //return PartialView("PartialViews/_PropertyList", propertyVM);
//    }

//    return View(viewModel);
//}

