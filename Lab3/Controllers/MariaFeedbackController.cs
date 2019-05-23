using Microsoft.AspNetCore.Mvc;

namespace Lab3.Controllers
{
    public class MariaFeedbackController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}