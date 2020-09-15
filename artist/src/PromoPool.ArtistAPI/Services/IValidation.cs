using PromoPool.ArtistAPI.Models;

namespace PromoPool.ArtistAPI.Services
{
    public interface IValidation
    {
        bool ValidateId(string id);

        bool ValidateNewArtistModel(NewArtist newArtist);

    }
}