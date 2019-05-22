using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Lab3.Models
{
    public class MongoFeedbackContext
    {
        private readonly IMongoDatabase database = null;
    
        public MongoFeedbackContext(IOptions<MongoSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            if (client != null)
                database = client.GetDatabase(settings.Value.Database);
        }

        public IMongoCollection<Feedback> Feedback => database.GetCollection<Feedback>("Feedback");
    }
}