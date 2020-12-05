using PromoPool.ArtistAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PromoPool.ArtistAPI.Managers
{
    public interface IArtistManager
    {
        Task<IEnumerable<Artist>> GetAllArtistsAsync();
        Task<IEnumerable<Artist>> FindAllArtistsByNameAsync(string artistName);
        Task<Artist> GetArtistByIdAsync(string id);
        Task<string> InsertArtistAsync(NewArtist newArtist);
        Task<bool> DeleteArtistByIdAsync(string id);
        Task<bool> DeleteAllArtistsAsync();
        Task<Artist> UpdateArtistAsync(string id, UpdateArtist updateArtist);
    }
}