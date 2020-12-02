using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PromoPool.ArtistAPI.Managers;
using PromoPool.ArtistAPI.Managers.Implementations;
using PromoPool.ArtistAPI.Models;
using PromoPool.ArtistAPI.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PromoPool.ArtistAPI.UnitTests.Managers
{
    [TestClass]
    public class ArtistManagerTests
    {
        
        private Mock<IMongoDBPersistance> mongoDBPersistance;
        private IArtistManager _artistManager;

        [TestInitialize]
        public void Setup()
        {
            mongoDBPersistance = new Mock<IMongoDBPersistance>();
            _artistManager = new ArtistManager(mongoDBPersistance.Object);

        }
        
        [TestMethod]
        public void GetArtistByIdAsync_Given_NoArtist_Should_Return_Null()
        {
            
            mongoDBPersistance
                .Setup(s => s.FindArtistByIdAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult((Artist)null));

            string id = "123e4567-e89b-12d3-a456-426652340000";

            var retval = _artistManager.GetArtistByIdAsync(id);

            Assert.IsNull(retval.Result);
        }

        [TestMethod]
        public void GetArtistByIdAsync_Given_Artist_Should_Return_Artist()
        {

            var artist = new Artist();

            mongoDBPersistance
                .Setup(s => s.FindArtistByIdAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(artist));

            string id = "123e4567-e89b-12d3-a456-426652340000";

            var retval = _artistManager.GetArtistByIdAsync(id);

            Assert.IsNotNull(retval);
        }

        [TestMethod]
        public async Task GetAllArtistsAsync_Given_Artists_Exist_Should_Return_List_Off_Artists()
        {
            IEnumerable<Artist> artists = new List<Artist>()
            {
                new Artist(),
                new Artist(),
                new Artist(),
                new Artist()
            };
            
            mongoDBPersistance
                .Setup(s => s.FindAllArtistsAsync())
                .Returns(Task.FromResult(artists));

            var retval = await _artistManager.GetAllArtistsAsync();

            Assert.IsNotNull(retval);
        }

        [TestMethod]
        public async Task GetAllArtistsAsync_Given_No_Artists_Exist_Should_Return_Null()
        {
            IEnumerable<Artist> artists = null;
            
            mongoDBPersistance
                .Setup(s => s.FindAllArtistsAsync())
                .Returns(Task.FromResult(artists));

            var retval = await _artistManager.GetAllArtistsAsync();

            Assert.IsNull(retval);
        }

        
        [TestMethod]
        public void InsertArtistAsync_Given_Artist_Should_Return_Guid_Id()
        {
            var artist = new NewArtist()
            {
                Name = "Primal Nature",
                ProfilePictureURL = "http://www.primalnature.com/artist.png",
                BeatportUrl = "http://www.beatport.com/primalnature/",
                SoundCloudUrl = "http://www.soundcloud.com/primalnature/"
            };

            var id = Guid.NewGuid().ToString();

            mongoDBPersistance
                .Setup(s => s.InsertOneArtistAsync(It.IsAny<Artist>()))
                .Returns(Task.FromResult(id));

            var retval = _artistManager.InsertArtistAsync(artist);

            Assert.IsTrue(Guid.TryParse(id,out _));
        }

        [TestMethod]
        public void DeleteArtistByIdAsync_Given_ID_Should_Delete_Artist()
        {

            mongoDBPersistance
                .Setup(s => s.DeleteOneArtistAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(true));

            string id = "123e4567-e89b-12d3-a456-426652340000";

            var retval = _artistManager.DeleteArtistByIdAsync(id);

            Assert.IsTrue(retval.Result);
        }

        [TestMethod]
        public void DeleteArtistByIdAsync_Given_Unknown_ID_Should_Not_Delete_Artist()
        {

            mongoDBPersistance
                .Setup(s => s.DeleteOneArtistAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(false));

            string id = "123e4567-e89b-12d3-a456-426652340000";

            var retval = _artistManager.DeleteArtistByIdAsync(id);

            Assert.IsFalse(retval.Result);
        }

    }
}
