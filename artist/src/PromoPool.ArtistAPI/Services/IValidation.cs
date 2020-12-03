using PromoPool.ArtistAPI.Models;

namespace PromoPool.ArtistAPI.Services
{
    public interface IValidation
    {
        ValidationMessage ValidateId(string id);

        ValidationMessage ValidateQueryString(string queryString, string queryStringPropertyName);

        ValidationMessage ValidateNewArtistModel(NewArtist newArtist);

    }
}