using System;
using PromoPool.LabelAPI.Models;
using PromoPool.LabelAPI.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace PromoPool.LabelAPI.Managers.Implementations
{
    public class LabelManager : ILabelManager
    {
        private readonly IMongoDBPersistance mongoDBPersistance;

        public LabelManager(IMongoDBPersistance mongoDBPersistance)
        {
            this.mongoDBPersistance = mongoDBPersistance;
        }

        public async Task<IEnumerable<Label>> GetAllLabelsAsync()
        {
            var labels = await mongoDBPersistance.FindAllLabelsAsync();

            if (labels?.Any() == true )
            {
                return labels;
            }

            return null;
        }

        public async Task<Label> GetLabelByIdAsync(string id)
        {

            var label = await mongoDBPersistance.FindLabelByIdAsync(Guid.Parse(id));
            
            if(label == null)
            {
                return null;
            }

            return label;
        }

        public async Task<string> InsertLabelAsync(NewLabel newLabel)
        {
            
            var label = new Label()
            {
                Name = newLabel.Name,
                PhoneNumber = newLabel.PhoneNumber,
                Url = newLabel.Url,
                Email = newLabel.Email,
                Address = new Address()
                {
                    AddressLine1 = newLabel.Address.AddressLine1,
                    AddressLine2 = newLabel.Address.AddressLine2,
                    AddressLine3 = newLabel.Address.AddressLine3,
                    City = newLabel.Address.City,
                    Locality = newLabel.Address.Locality,
                    PostalCode = newLabel.Address.PostalCode,
                    Country = newLabel.Address.Country

                }

            };

            var labelId = await mongoDBPersistance.InsertOneLabelAsync(label);

            return labelId;
        }
    }
}
