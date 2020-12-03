using PromoPool.ArtistAPI.Models;
using System;

namespace PromoPool.ArtistAPI.Services.Implementations
{
    public class Validation : IValidation
    {

        public ValidationMessage ValidateId(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new ValidationMessage()
                {
                    resultValid = false,
                    message = "Id is null or empty!"
                };
            }

            if (!Guid.TryParse(id, out _))
            {
                return new ValidationMessage()
                {
                    resultValid = false,
                    message = "Id is not in a Guid format!"
                };
            }

            return new ValidationMessage()
            {
                resultValid = true
            };
        }

        public ValidationMessage ValidateNewArtistModel(NewArtist newArtist)
        {
            if (newArtist == null)
            {
                return new ValidationMessage()
                {
                    resultValid = false,
                    message = "Artist Model not provided!"
                };

            }

            return new ValidationMessage()
            {
                resultValid = true,
            };

        }
    }
}