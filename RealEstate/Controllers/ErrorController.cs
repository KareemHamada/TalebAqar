﻿namespace RealEstate.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult NotFound()
        {
            Response.StatusCode = 404;
            return View();
        }
    }
}
