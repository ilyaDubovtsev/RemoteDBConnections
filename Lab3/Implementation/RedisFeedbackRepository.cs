using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab3.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Lab3.Implementation
{
    public class RedisFeedbackRepository : IRedisFeedbackRepository
    {
        private readonly IDatabase database;
        private readonly string redisKey;

        public RedisFeedbackRepository(IOptions<RedisSettings> settings)
        {
            var redisConnection = ConnectionMultiplexer.Connect(settings.Value.ConnectionString);
            database = redisConnection.GetDatabase();
            redisKey = settings.Value.Database;
        }
       
        public Task<IEnumerable<Feedback>> ReadAll()
        {
            var hashEntries = database.HashScan(redisKey);
            return Task.FromResult(hashEntries.Select(x => Deserialize(x.Value)));
        }

        public Task Create(Feedback item)
        {
            return Task.FromResult(database.HashSet(redisKey, item.Id.ToString(), Serialize(item)));
        }

        public Task<Feedback> Read(Guid id)
        {
            var redisValue = database.HashGet(redisKey, id.ToString());
            return Task.FromResult(Deserialize(redisValue.ToString()));
        }

        public Task<bool> Update(Feedback item)
        {
            return Task.FromResult(database.HashSet(redisKey, item.Id.ToString(), Serialize(item)));
        }

        public Task<bool> Delete(Guid id)
        {
            return Task.FromResult(database.HashDelete(redisKey, id.ToString()));
        }

        public Task<bool> DeleteAll()
        {
            throw new NotImplementedException();
        }

        private string Serialize(Feedback feedback)
        {
            return JsonConvert.SerializeObject(feedback);
        }

        private Feedback Deserialize(string serialized)
        {
            return JsonConvert.DeserializeObject<Feedback>(serialized);
        }
    }
}