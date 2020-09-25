using Microsoft.AspNetCore.Mvc;

namespace Test.Platform.Wms.Api.Controllers
{
    public class IventoryController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}