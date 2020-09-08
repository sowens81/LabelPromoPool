using PromoPool.LabelAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PromoPool.LabelAPI.Managers
{
    public interface ILabelManager
    {
        Task<IEnumerable<Label>> GetAllLabelsAsync();
        Task<Label> GetLabelByIdAsync(string id);
        Task<string> InsertLabelAsync(NewLabel newLabel);
    }
}