using System;
using System.Linq;
using System.Threading.Tasks;
using Lab3.Implementation;
using Lab3.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lab3.Controllers
{
    [Route("mongo")]
    public class MongoFeedbackController : Controller
    {
        private readonly IFeedbackRepository feedbackRepository;

        public MongoFeedbackController(IMongoFeedbackRepository feedbackRepository)
        {
            this.feedbackRepository = feedbackRepository;
        }

        [HttpGet("")]
        public async Task<ViewResult> Index()
        {
            var allFeedback = await feedbackRepository.ReadAll().ConfigureAwait(true);
            return View(allFeedback.OrderByDescending(f => f.UpdatedAt));
        }

        [HttpPost("new")]
        public IActionResult New()
        {
            var form = Request.Form;
            var feedback = new Models.Feedback
            {
                Id = Guid.NewGuid(),
                Name = form["Name"],
                Text = form["Text"],
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            feedbackRepository.Create(feedback);

            return RedirectToAction("Index");
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("edit")]
        public IActionResult Edit()
        {
            var form = Request.Form;

            var feedback = new Models.Feedback
            {
                Id = Guid.Parse(form["Id"]),
                Name = form["Name"],
                Text = form["Text"],
            };
            feedbackRepository.Update(feedback);
            return RedirectToAction("Index");
        }

        [HttpGet("update/{id}")]
        public async Task<ViewResult> Update(string id)
        {
            var feedback = await feedbackRepository.Read(Guid.Parse(id)).ConfigureAwait(true);
            return View(feedback);
        }

        [HttpGet("{id}")]
        public async Task<ViewResult> Show(string id)
        {
            var feedback = await feedbackRepository.Read(Guid.Parse(id)).ConfigureAwait(true);
            return View(feedback);
        }

        [HttpGet("delete/{id}")]
        public IActionResult Delete(string id)
        {
            feedbackRepository.Delete(Guid.Parse(id));
            return RedirectToAction("Index");
        }
    }
}