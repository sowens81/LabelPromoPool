using System;
using System.Runtime.Serialization;

namespace PromoPool.LabelAPI.Exceptions
{
     [Serializable]
    public class MissingIdException : ArgumentException, ISerializable
    {
        public MissingIdException(string message, string parameter) : base(message, parameter)
        { }
    }
}
