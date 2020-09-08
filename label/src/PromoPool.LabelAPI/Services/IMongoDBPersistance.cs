using System;
using PromoPool.LabelAPI.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace PromoPool.LabelAPI.Services
{
    public interface IMongoDBPersistance
    {
        Task<IEnumerable<Label>> FindAllLabelsAsync();
        Task<Label>  FindLabelByIdAsync(Guid id);
        Task<string> InsertOneLabelAsync(Label label);
    }

}
