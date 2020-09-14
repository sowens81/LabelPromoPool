using Microsoft.VisualStudio.TestTools.UnitTesting;
using PromoPool.LabelAPI.Models;
using System;

namespace PromoPool.LabelAPI.UnitTests.Models
{
    [TestClass]
    public class LabelTests
    {
        [TestMethod]
        public void Assert_Id_Is_Guid()
        {
            //Arrange
            var label = new Label();

            //Act
            var labelId = label.Id.ToString();
            //Assert
            Assert.IsTrue(Guid.TryParse(labelId, out _));
        }
    }
}
