using PromoPool.ArtistAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PromoPool.ArtistAPI.Managers
{
    public interface IArtistManager
    {
        Task<IEnumerable<Artist>> GetAllArtistsAsync();
        Task<Artist> GetArtistByIdAsync(string id);
        Task<string> InsertArtistAsync(NewArtist newArtist);
        Task<bool> DeleteArtistByIdAsync(string id);
    }
}