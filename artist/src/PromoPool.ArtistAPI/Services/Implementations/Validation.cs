using PromoPool.ArtistAPI.Exceptions;
using PromoPool.ArtistAPI.Models;
using System;

namespace PromoPool.ArtistAPI.Services.Implementations
{
    public class Validation : IValidation
    {

        public bool ValidateId(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new MissingIdException("Id is null or empty", nameof(id));
            }

            if (!Guid.TryParse(id, out _))
            {
                throw new MismatchIdException("Id is not in a Guid format", nameof(id));
            }

            return true;
        }

        public bool ValidateNewArtistModel(NewArtist newArtist)
        {
            if (newArtist == null)
            {
                throw new ArgumentException("No artist", nameof(newArtist));
            }

            return true;

        }
    }
}