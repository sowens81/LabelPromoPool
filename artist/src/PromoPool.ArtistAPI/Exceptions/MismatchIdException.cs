using System;
using System.Runtime.Serialization;

namespace PromoPool.ArtistAPI.Exceptions
{
    [Serializable]
    public class MismatchIdException : ArgumentException, ISerializable
    {
        public MismatchIdException(string message, string parameter) : base(message, parameter)
        { }
    }
}
