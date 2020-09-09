using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PromoPool.LabelAPI.Managers;
using PromoPool.LabelAPI.Managers.Implementations;
using PromoPool.LabelAPI.Models;
using PromoPool.LabelAPI.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PromoPool.LabelAPI.UnitTests.Managers
{
    [TestClass]
    public class LabelManagerTests
    {
        
        private Mock<IMongoDBPersistance> mongoDBPersistance;
        private ILabelManager _labelManager;

        [TestInitialize]
        public void Setup()
        {
            mongoDBPersistance = new Mock<IMongoDBPersistance>();
            _labelManager = new LabelManager(mongoDBPersistance.Object);

        }
        
        [TestMethod]
        public void GetLabelByIdAsync_Given_NoLabel_Should_Return_Null()
        {
            
            mongoDBPersistance
                .Setup(s => s.FindLabelByIdAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult((Label)null));

            string id = "123e4567-e89b-12d3-a456-426652340000";

            var retval = _labelManager.GetLabelByIdAsync(id);

            Assert.IsNull(retval.Result);
        }

        [TestMethod]
        public void GetLabelByIdAsync_Given_Label_Should_Return_Label()
        {

            var label = new Label();

            mongoDBPersistance
                .Setup(s => s.FindLabelByIdAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(label));

            string id = "123e4567-e89b-12d3-a456-426652340000";

            var retval = _labelManager.GetLabelByIdAsync(id);

            Assert.IsNotNull(retval);
        }

        [TestMethod]
        public async Task GetAllLabelsAsync_Given_Labels_Exist_Should_Return_List_Off_Labels()
        {
            IEnumerable<Label> labels = new List<Label>()
            {
                new Label(),
                new Label(),
                new Label(),
                new Label()
            };
            
            mongoDBPersistance
                .Setup(s => s.FindAllLabelsAsync())
                .Returns(Task.FromResult(labels));

            var retval = await _labelManager.GetAllLabelsAsync();

            Assert.IsNotNull(retval);
        }

        [TestMethod]
        public async Task GetAllLabelsAsync_Given_No_Labels_Exist_Should_Return_Null()
        {
            IEnumerable<Label> labels = null;
            
            mongoDBPersistance
                .Setup(s => s.FindAllLabelsAsync())
                .Returns(Task.FromResult(labels));

            var retval = await _labelManager.GetAllLabelsAsync();

            Assert.IsNull(retval);
        }

        
        [TestMethod]
        public void InsertLabelAsync_Given_Label_Should_Return_Guid_Id()
        {
            var label = new NewLabel()
            {
                Name = "labelname",
                PhoneNumber = "+44 (0) 7427 66 09 03",
                Email = "email@email.com",
                Url = "http://www.domain.com",
                Address = new RequestAddress()
                {
                    AddressLine1 = "Addessline 1",
                    AddressLine2 = "Address Line 2",
                    AddressLine3 = "Address Line 3",
                    City = "City",
                    Locality = "Locality",
                    PostalCode = "PostalCode",
                    Country = "Country"
                }
            };

            var id = Guid.NewGuid().ToString();

            mongoDBPersistance
                .Setup(s => s.InsertOneLabelAsync(It.IsAny<Label>()))
                .Returns(Task.FromResult(id));

            var retval = _labelManager.InsertLabelAsync(label);

            Assert.IsTrue(Guid.TryParse(id,out _));
        }
    }
}
