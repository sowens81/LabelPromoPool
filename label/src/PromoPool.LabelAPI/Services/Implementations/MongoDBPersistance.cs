using System;
using MongoDB.Driver;
using PromoPool.LabelAPI.Models;
using PromoPool.LabelAPI.Settings;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace PromoPool.LabelAPI.Services.Implementations
{
    public class MongoDBPersistance : IMongoDBPersistance
    {
        private readonly IMongoCollection<Label> _collection;

        public MongoDBPersistance(IDatabaseSettings settings)
        {
            
            var client = new MongoClient(settings.ConnectionString);

            var database = client.GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<Label>(settings.CollectionName);
        }

        public async Task<IEnumerable<Label>> FindAllLabelsAsync()
        {
            return await _collection.Find(p => true)?.ToListAsync();
        }

        public async Task<Label> FindLabelByIdAsync(Guid id)
        { 
            return await _collection.Find<Label>(label => label.Id == id)?.SingleAsync();
        }

        public async Task<string> InsertOneLabelAsync(Label label)
        {
            await _collection.InsertOneAsync(label);

            return label.Id.ToString();
        }

    }
}
