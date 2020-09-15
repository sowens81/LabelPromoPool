using PromoPool.LabelAPI.Exceptions;
using PromoPool.LabelAPI.Models;
using System;

namespace PromoPool.LabelAPI.Services.Implementations
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

        public bool ValidateNewLabelModel(NewLabel newLabel)
        {
            if (newLabel == null)
            {
                throw new ArgumentException("No label", nameof(newLabel));
            }

            return true;

        }
    }
}