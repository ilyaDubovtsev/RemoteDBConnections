using Microsoft.AspNetCore.Mvc;

namespace Lab3.Controllers
{
    public class RedisFeedbackController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
        
    }
}