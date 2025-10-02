using Microsoft.AspNetCore.Mvc;

namespace lab_infrastructure.Controllers
{
    public class StatisticsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
