using PromoPool.ArtistAPI.Models;

namespace PromoPool.ArtistAPI.Services
{
    public interface IValidation
    {
        ValidationMessage ValidateId(string id);

        ValidationMessage ValidateNewArtistModel(NewArtist newArtist);

    }
}