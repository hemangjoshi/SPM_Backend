using Microsoft.AspNetCore.Mvc;

namespace SPM_Backend.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
