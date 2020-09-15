using System;
using MongoDB.Driver;
using PromoPool.ArtistAPI.Models;
using PromoPool.ArtistAPI.Settings;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace PromoPool.ArtistAPI.Services.Implementations
{
    public class MongoDBPersistance : IMongoDBPersistance
    {
        private readonly IMongoCollection<Artist> _collection;

        public MongoDBPersistance(IDatabaseSettings settings)
        {
            
            var client = new MongoClient(settings.ConnectionString);

            var database = client.GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<Artist>(settings.CollectionName);
        }

        public async Task<IEnumerable<Artist>> FindAllArtistsAsync()
        {
            return await _collection.Find(p => true)?.ToListAsync();
        }

        public async Task<Artist> FindArtistByIdAsync(Guid id)
        { 
            return await _collection.Find<Artist>(artist => artist.Id == id)?.SingleAsync();
        }

        public async Task<string> InsertOneArtistAsync(Artist artist)
        {
            await _collection.InsertOneAsync(artist);

            return artist.Id.ToString();
        }

    }
}
