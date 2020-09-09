using PromoPool.LabelAPI.Models;

namespace PromoPool.LabelAPI.Services
{
    public interface IValidation
    {
        bool ValidateId(string id);

        bool ValidateNewLabelModel(NewLabel newLabel);

    }
}