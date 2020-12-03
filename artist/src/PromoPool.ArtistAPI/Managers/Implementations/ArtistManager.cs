using System;
using PromoPool.ArtistAPI.Models;
using PromoPool.ArtistAPI.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace PromoPool.ArtistAPI.Managers.Implementations
{
    public class ArtistManager : IArtistManager
    {
        private readonly IMongoDBPersistance mongoDBPersistance;

        public ArtistManager(IMongoDBPersistance mongoDBPersistance)
        {
            this.mongoDBPersistance = mongoDBPersistance;
        }

        public async Task<IEnumerable<Artist>> GetAllArtistsAsync()
        {
            var artists = await mongoDBPersistance.FindAllArtistsAsync();

            if ( artists != null  && artists.Any() )
            {
                return artists;
            }

            return null;
        }

        public async Task<IEnumerable<Artist>> FindAllArtistsByNameAsync(string artistName)
        {
            var artists = await mongoDBPersistance.FindAllArtistsByNameAsync(artistName);

            if (artists != null && artists.Any())
            {
                return artists;
            }

            return null;
        }

        public async Task<Artist> GetArtistByIdAsync(string id)
        {

            var artist = await mongoDBPersistance.FindArtistByIdAsync(Guid.Parse(id));
            
            if(artist == null)
            {
                return null;
            }

            return artist;
        }

        public async Task<string> InsertArtistAsync(NewArtist newArtist)
        {
            
            var artist = new Artist()
            {
                Name = newArtist.Name,
                ProfilePictureURL = newArtist.ProfilePictureURL,
                BeatportUrl = newArtist.BeatportUrl,
                SoundCloudUrl = newArtist.SoundCloudUrl

            };

            var artistId = await mongoDBPersistance.InsertOneArtistAsync(artist);

            return artistId;
        }

        public async Task<bool> DeleteArtistByIdAsync(string id)
        {
           var deleteSuccessful = await mongoDBPersistance.DeleteOneArtistAsync(Guid.Parse(id));

           return deleteSuccessful;
        }

        public async Task<bool> DeleteAllArtistsAsync()
        {

            var deleteSuccessful = await mongoDBPersistance.DeleteAllArtistsAsync();

            return deleteSuccessful;
        }

    }
}
