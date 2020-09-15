using Microsoft.VisualStudio.TestTools.UnitTesting;
using PromoPool.ArtistAPI.Models;
using System;

namespace PromoPool.ArtistAPI.UnitTests.Models
{
    [TestClass]
    public class ArtistTests
    {
        [TestMethod]
        public void Assert_Id_Is_Guid()
        {
            //Arrange
            var artist = new Artist();

            //Act
            var artistId = artist.Id.ToString();
            //Assert
            Assert.IsTrue(Guid.TryParse(artistId, out _));
        }
    }
}
