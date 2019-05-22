using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lab3.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Lab3.Implementation
{
    class MongoFeedbackRepository : IMongoFeedbackRepository
    {
        private readonly MongoFeedbackContext context;

        public MongoFeedbackRepository(IOptions<MongoSettings> settings)
        {
            context = new MongoFeedbackContext(settings);
        }

        public async Task<IEnumerable<Feedback>> ReadAll()
        {
            return await context.Feedback
                .Find(_ => true)
                .ToListAsync();
        }

        public async Task Create(Feedback feedback)
        {
            try
            {
                await context.Feedback.InsertOneAsync(feedback);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Feedback> Read(Guid id)
        {
            return await context.Feedback
                .Find(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> Update(Feedback feedback)
        {
            var filter = Builders<Feedback>.Filter.Eq(f => f.Id, feedback.Id);
            var updatedFeedback = Builders<Feedback>.Update
                .Set(f => f.Name, feedback.Name)
                .Set(f => f.Text, feedback.Text)
                .CurrentDate(s => s.UpdatedAt);

            var updateResult = await context.Feedback.UpdateOneAsync(filter, updatedFeedback);

            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> Delete(Guid id)
        {
            var deleteResult = await context.Feedback
                .DeleteOneAsync(
                    Builders<Feedback>.Filter.Eq(f => f.Id, id)
                );

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

        public async Task<bool> DeleteAll()
        {
            var actionResult 
                = await context.Feedback.DeleteManyAsync(new BsonDocument());

            return actionResult.IsAcknowledged && actionResult.DeletedCount > 0;
        }
    }
}