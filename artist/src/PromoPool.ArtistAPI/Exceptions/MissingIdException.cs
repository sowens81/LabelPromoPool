using System;

namespace PromoPool.ArtistAPI.Exceptions
{
    [Serializable]
    public class MissingIdException : ArgumentException
    {
        public MissingIdException(string message, string parameter) : base(message, parameter)
        {
        }
    }
}
