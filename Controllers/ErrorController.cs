using Microsoft.AspNetCore.Mvc;

namespace SimpleNotesApp.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/{statusCode?}")]
        public IActionResult HandleError(int? statusCode = null)
        {
            if (statusCode == 404 || Response.StatusCode == 404)
            {
                return View("~/Views/Shared/404.cshtml");
            }

            return View("Error");
        }
    }
}