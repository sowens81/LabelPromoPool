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

            // I need to build out an index that enables case insenseitive text searches of mongdb.

        }

        public async Task<IEnumerable<Artist>> FindAllArtistsAsync()
        {
            return await _collection.Find(p => true)?.ToListAsync();
        }

        public async Task<IEnumerable<Artist>> FindAllArtistsByNameAsync(string artistName)
        {
            
            var query = Builders<Artist>.Filter.Eq(x => x.Name, artistName);
            return await _collection.Find(query)?.ToListAsync();

            // I need to replace this method to perform a case insensitve search for the artistname and return a set of records.
        }

        public async Task<Artist> FindArtistByIdAsync(Guid id)
        {
            var query = Builders<Artist>.Filter.Eq(x => x.Id, id);
            return await _collection.Find<Artist>(query)?.SingleAsync();
        }

        public async Task<string> InsertOneArtistAsync(Artist artist)
        {
            await _collection.InsertOneAsync(artist);

            return artist.Id.ToString();
        }
        public async Task<bool> DeleteOneArtistAsync(Guid id)
        {
            var query = Builders<Artist>.Filter.Eq(x => x.Id, id);
            var result = await _collection.DeleteOneAsync(query);
            return await Task.FromResult(result.DeletedCount == 1);
            
        }

        public async Task<bool> DeleteAllArtistsAsync()
        {
            var query = Builders<Artist>.Filter.Where(x => true);
            var result = await _collection.DeleteManyAsync(query);
            return await Task.FromResult(result.DeletedCount > 0);
        }

        public async Task<Artist> UpdateArtistByIdAsync(Guid id, Artist artist)
        {
            var query = Builders<Artist>.Filter.Eq(x => x.Id, id);
            var result = await _collection.ReplaceOneAsync(query, artist);

            if (await Task.FromResult(result.IsAcknowledged == false))
            {
                return null;
            }

            return artist;
                
        }
    }
}
