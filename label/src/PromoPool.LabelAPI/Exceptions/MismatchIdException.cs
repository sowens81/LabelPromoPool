using System;
using System.Runtime.Serialization;

namespace PromoPool.LabelAPI.Exceptions
{
    [Serializable]
    public class MismatchIdException : ArgumentException, ISerializable
    {
        public MismatchIdException(string message, string parameter) : base(message, parameter)
        { }
    }
}
