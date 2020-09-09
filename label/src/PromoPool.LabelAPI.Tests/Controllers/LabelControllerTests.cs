using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PromoPool.LabelAPI.Controllers;
using PromoPool.LabelAPI.Exceptions;
using PromoPool.LabelAPI.Managers;
using PromoPool.LabelAPI.Models;
using PromoPool.LabelAPI.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PromoPool.LabelAPI.UnitTests.Controllers
{
    [TestClass]
    public class LabelControllerTests
    {
 
        private Mock<ILabelManager> labelManager;
        private Mock<ILogger<LabelController>> logger;
        private LabelController _labelController;

        [TestInitialize]
        public void Setup()
        {
            this.labelManager = new Mock<ILabelManager>();
            this.logger = new Mock<ILogger<LabelController>>();
            _labelController = new LabelController(labelManager.Object, logger.Object, new Validation());

        }

        [TestMethod]
        [ExpectedException(typeof(MissingIdException))]
        public async Task GetLabelByIdAsync_Given_NullId_Should_ThrowExceptionAsync()
        {
            await _labelController.GetLabelByIdAsync(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(MismatchIdException))]
        public async Task GetLabelByIdAsync_Given_Non_Guid_Should_ThrowExceptionAsync()
        {
            string id = "abc$_123";
            await _labelController.GetLabelByIdAsync(id);
        }

        [TestMethod]
        public void GetLabelByIdAsync_Given_Id_As_Guid_String_Should_Return_Label()
        {
            var id = Guid.NewGuid().ToString();

            var label = new Label();

            labelManager
                .Setup(s => s.GetLabelByIdAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(label));

            var retval = _labelController.GetLabelByIdAsync(id);

            Assert.IsNotNull(retval);
        }

        [TestMethod]
        public async Task AddLabelAsync_Given_NewLabel_Should_Return_Id()
        {
            var label = new NewLabel()
            {
                Name = "labelname",
                PhoneNumber = "+44 (0) 7427 66 09 03",
                Email = "email@email.com",
                Url = "http://www.domain.com",
                Address = new NewAddress()
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

            string id = Guid.NewGuid().ToString();

            labelManager
                .Setup(s => s.InsertLabelAsync(It.IsAny<NewLabel>()))
                .Returns(Task.FromResult(id));

            var retval = await _labelController.AddLabelAsync(label) as CreatedResult;

            Assert.AreEqual(id, retval.Value);
        }

        [TestMethod]
        public async Task GetLabelsAsync_Given_Labels_In_DB_Exist_Should_Return_Collection_Of_Labels()
        {
            IEnumerable<Label> labels = new List<Label>(){
                new Label(),
                new Label(),
                new Label(),
                new Label()
            };

            labelManager
                .Setup(s => s.GetAllLabelsAsync())
                .Returns(Task.FromResult(labels));

            var retval = await _labelController.GetLabelsAsync() as OkObjectResult;

            Assert.IsNotNull(retval.Value);
        }

        [TestMethod]
        public async Task GetLabelsAsync_Given_Labels_In_DB_Exist_Should_Return_200_Ok()
        {
            IEnumerable<Label> labels = new List<Label>(){
                new Label(),
                new Label(),
                new Label(),
                new Label()
            };

            labelManager
                .Setup(s => s.GetAllLabelsAsync())
                .Returns(Task.FromResult(labels));

            var retval = await _labelController.GetLabelsAsync() as OkObjectResult;

            Assert.IsNotNull(retval.Value);
            Assert.AreEqual(200, retval.StatusCode);
        }

        [TestMethod]
        public async Task GetLabelsAsync_Given_No_Labels_In_DB_Exist_Should_Return_404_NotFound()
        {
            IEnumerable<Label> labels = null;

            labelManager
                .Setup(s => s.GetAllLabelsAsync())
                .Returns(Task.FromResult(labels));

            var retval = await _labelController.GetLabelsAsync() as NotFoundResult;


            Assert.AreEqual(404, retval.StatusCode);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task AddLabelAsync_Given_Null_NewLabel_Should_ThrowException()
        {
            await _labelController.AddLabelAsync((NewLabel)null);
        }

        [TestMethod]
        public async Task AddLabel_Given_NewLabel_Should_Return_201_CreatedResult()
        {
            var label = new NewLabel()
            {
                Name = "labelname",
                PhoneNumber = "+44 (0) 7427 66 09 03",
                Email = "email@email.com",
                Url = "http://www.domain.com",
                Address = new NewAddress()
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

            labelManager
                .Setup(s => s.InsertLabelAsync(It.IsAny<NewLabel>()))
                .Returns(Task.FromResult(id));

            var retval = await _labelController.AddLabelAsync(label) as CreatedResult;

            Assert.IsNotNull(retval);
            Assert.AreEqual(201, retval.StatusCode);
        }

        [TestMethod]
        public async Task AddLabel_Given_NewLabel_ModelState_Not_Valid_Return_400_BadRequest()
        {
            var label = new NewLabel()
            {
                Name = "",
                PhoneNumber = "",
                Email = "",
                Url = "",
                Address = new NewAddress()
                {
                    AddressLine1 = "",
                    AddressLine2 = "",
                    AddressLine3 = "Address Line 3",
                    City = "",
                    Locality = "",
                    PostalCode = "",
                    Country = ""
                }
            };

            var retval = await _labelController.AddLabelAsync(label);

            Assert.IsInstanceOfType(retval, typeof(BadRequestResult));
        }

        [TestMethod]
        public async Task AddLabel_Given_NewLabel_Not_Added_Return_400_BadRequestResult()
        {
            var label = new NewLabel()
            {
                Name = "labelname",
                PhoneNumber = "+44 (0) 7427 66 09 03",
                Email = "email@email.com",
                Url = "http://www.domain.com",
                Address = new NewAddress()
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

            labelManager
                .Setup(s => s.InsertLabelAsync(It.IsAny<NewLabel>()))
                .Returns(Task.FromResult((string)null));

            var retval = await _labelController.AddLabelAsync(label) as BadRequestResult;

            Assert.IsNotNull(retval);
            Assert.AreEqual(400, retval.StatusCode);
        }
    }
}
