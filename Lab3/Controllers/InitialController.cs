using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab3.Implementation;
using Lab3.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lab3.Controllers
{
    [Route("Init")]
    public class InitialController : Controller
    {
        private readonly IMongoFeedbackRepository mongoFeedbackRepository;

        public InitialController(IMongoFeedbackRepository mongoFeedbackRepository)
        {
            this.mongoFeedbackRepository = mongoFeedbackRepository;
        }

        [HttpGet("{setting}")]
        public string Initialize(string setting)
        {
            if (setting == "mongo")
            {
                mongoFeedbackRepository.DeleteAll();

                for (var i = 0; i < 10; i++)
                {
                    mongoFeedbackRepository.Create(
                        new Models.Feedback()
                        {
                            Id = Guid.NewGuid(),
                            Name = $"Feedback {i}",
                            Text = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now
                        }
                    );
                }

                return "Done";
            }
            return "Unknown";
        }

        [HttpGet("mongoall")]
        public async Task<string> GetAll()
        {
            var allFeedback = await mongoFeedbackRepository.ReadAll().ConfigureAwait(true);
            var stringBuilder = new StringBuilder();
            foreach (var feedback in allFeedback)
            {
                stringBuilder.Append($"{feedback.Id} {feedback.Name} {feedback.Text} \n");
            }
            return stringBuilder.ToString();
        }
    }
}