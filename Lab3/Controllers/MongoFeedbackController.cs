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
        private readonly IMongoFeedbackRepository mongoFeedbackRepository;

        public MongoFeedbackController(IMongoFeedbackRepository mongoFeedbackRepository)
        {
            this.mongoFeedbackRepository = mongoFeedbackRepository;
        }

        [HttpGet("")]
        public async Task<ViewResult> Index()
        {
            var allFeedback = await mongoFeedbackRepository.ReadAll().ConfigureAwait(true);
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

            mongoFeedbackRepository.Create(feedback);

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
            Console.WriteLine("****sadsd***");

            Console.WriteLine(form["Id"]);
            var feedback = new Models.Feedback
            {
                Id = Guid.Parse(form["Id"]),
                Name = form["Name"],
                Text = form["Text"],
            };
            mongoFeedbackRepository.Update(feedback);
            return RedirectToAction("Index");
        }

        [HttpGet("update/{id}")]
        public async Task<ViewResult> Update(string id)
        {
            var feedback = await mongoFeedbackRepository.Read(Guid.Parse(id)).ConfigureAwait(true);
            return View(feedback);
        }

        [HttpGet("{id}")]
        public async Task<ViewResult> Show(string id)
        {
            var feedback = await mongoFeedbackRepository.Read(Guid.Parse(id)).ConfigureAwait(true);
            return View(feedback);
        }

        [HttpGet("delete/{id}")]
        public IActionResult Delete(string id)
        {
            mongoFeedbackRepository.Delete(Guid.Parse(id));
            return RedirectToAction("Index");
        }
    }
}