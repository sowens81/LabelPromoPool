using System;
using PromoPool.ArtistAPI.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace PromoPool.ArtistAPI.Services
{
    public interface IMongoDBPersistance
    {
        Task<IEnumerable<Artist>> FindAllArtistsAsync();
        Task<Artist>  FindArtistByIdAsync(Guid id);
        Task<string> InsertOneArtistAsync(Artist artist);
        Task<bool> DeleteOneArtistAsync(Guid id);
    }

}
