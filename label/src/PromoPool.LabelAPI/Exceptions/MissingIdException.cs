using System;
namespace PromoPool.LabelAPI.Exceptions
{
     [Serializable()]
    public class MissingIdException : ArgumentException
    {
        public MissingIdException(string message, string parameter) : base(message, parameter)
        { }
    }
}
