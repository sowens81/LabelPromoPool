using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PromoPool.ArtistAPI.Controllers;
using PromoPool.ArtistAPI.Managers;
using PromoPool.ArtistAPI.Models;
using PromoPool.ArtistAPI.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PromoPool.ArtistAPI.UnitTests.Controllers
{
    [TestClass]
    public class ArtistControllerTests
    {
 
        private Mock<IArtistManager> artistManager;
        private Mock<ILogger<ArtistController>> logger;
        private ArtistController _artistController;

        [TestInitialize]
        public void Setup()
        {
            this.artistManager = new Mock<IArtistManager>();
            this.logger = new Mock<ILogger<ArtistController>>();
            _artistController = new ArtistController(artistManager.Object, logger.Object, new Validation());

        }

        [TestMethod]
        [ExpectedException(typeof(MissingIdException))]
        public async Task GetArtistByIdAsync_Given_NullId_Should_ThrowExceptionAsync()
        {
            await _artistController.GetArtistByIdAsync(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(MismatchIdException))]
        public async Task GetArtistByIdAsync_Given_Non_Guid_Should_ThrowExceptionAsync()
        {
            string id = "abc$_123";
            await _artistController.GetArtistByIdAsync(id);
        }

        [TestMethod]
        public async Task GetArtistByIdAsync_Given_Id_As_Guid_String_Should_Return_Artist()
        {
            var id = Guid.NewGuid().ToString();

            var artist = new Artist();

            artistManager
                .Setup(s => s.GetArtistByIdAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(artist));

            var retval = await _artistController.GetArtistByIdAsync(id);

            Assert.IsNotNull(retval);
        }

        [TestMethod]
        public async Task AddArtistAsync_Given_NewArtist_Should_Return_Id()
        {
            var artist = new NewArtist()
            {
                Name = "Primal Nature",
                ProfilePictureURL = "http://www.primalnature.com/artist.png",
                BeatportUrl = "http://www.beatport.com/primalnature/",
                SoundCloudUrl = "http://www.soundcloud.com/primalnature/"
            };

            string id = Guid.NewGuid().ToString();

            artistManager
                .Setup(s => s.InsertArtistAsync(It.IsAny<NewArtist>()))
                .Returns(Task.FromResult(id));

            var retval = await _artistController.AddArtistAsync(artist) as CreatedResult;

            Assert.AreEqual(id, retval.Value);
        }

        [TestMethod]
        public async Task GetArtistsAsync_Given_Artists_In_DB_Exist_Should_Return_Collection_Of_Artists()
        {
            IEnumerable<Artist> artists = new List<Artist>(){
                new Artist(),
                new Artist(),
                new Artist(),
                new Artist()
            };

            artistManager
                .Setup(s => s.GetAllArtistsAsync())
                .Returns(Task.FromResult(artists));

            var retval = await _artistController.GetArtistsAsync() as OkObjectResult;

            Assert.IsNotNull(retval.Value);
        }

        [TestMethod]
        public async Task GetArtistsAsync_Given_Artists_In_DB_Exist_Should_Return_200_Ok()
        {
            IEnumerable<Artist> artists = new List<Artist>(){
                new Artist(),
                new Artist(),
                new Artist(),
                new Artist()
            };

            artistManager
                .Setup(s => s.GetAllArtistsAsync())
                .Returns(Task.FromResult(artists));

            var retval = await _artistController.GetArtistsAsync() as OkObjectResult;

            Assert.IsNotNull(retval.Value);
            Assert.AreEqual(200, retval.StatusCode);
        }

        [TestMethod]
        public async Task GetArtistsAsync_Given_No_Artists_In_DB_Exist_Should_Return_404_NotFound()
        {
            IEnumerable<Artist> artists = null;

            artistManager
                .Setup(s => s.GetAllArtistsAsync())
                .Returns(Task.FromResult(artists));

            var retval = await _artistController.GetArtistsAsync() as NotFoundResult;


            Assert.AreEqual(404, retval.StatusCode);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task AddArtistAsync_Given_Null_NewArtist_Should_ThrowException()
        {
            await _artistController.AddArtistAsync((NewArtist)null);
        }

        [TestMethod]
        public async Task AddArtist_Given_NewArtist_Should_Return_201_CreatedResult()
        {
            var artist = new NewArtist()
            {
                Name = "Primal Nature",
                ProfilePictureURL = "http://www.primalnature.com/artist.png",
                BeatportUrl = "http://www.beatport.com/primalnature/",
                SoundCloudUrl = "http://www.soundcloud.com/primalnature/"
            };

            var id = Guid.NewGuid().ToString();

            artistManager
                .Setup(s => s.InsertArtistAsync(It.IsAny<NewArtist>()))
                .Returns(Task.FromResult(id));

            var retval = await _artistController.AddArtistAsync(artist) as CreatedResult;

            Assert.IsNotNull(retval);
            Assert.AreEqual(201, retval.StatusCode);
        }

        [TestMethod]
        public async Task AddArtist_Given_NewArtist_ModelState_Not_Valid_Return_400_BadRequest()
        {
            var artist = new NewArtist()
            {
                Name = "",
                ProfilePictureURL = "",
                BeatportUrl = "",
                SoundCloudUrl = ""
            };

            var retval = await _artistController.AddArtistAsync(artist);

            Assert.IsInstanceOfType(retval, typeof(BadRequestResult));
        }

        [TestMethod]
        public async Task AddArtist_Given_NewArtist_Not_Added_Return_400_BadRequestResult()
        {
            var artist = new NewArtist()
            {
                Name = "Primal Nature",
                ProfilePictureURL = "http://www.primalnature.com/artist.png",
                BeatportUrl = "http://www.beatport.com/primalnature/",
                SoundCloudUrl = "http://www.soundcloud.com/primalnature/"
            };

            var id = Guid.NewGuid().ToString();

            artistManager
                .Setup(s => s.InsertArtistAsync(It.IsAny<NewArtist>()))
                .Returns(Task.FromResult((string)null));

            var retval = await _artistController.AddArtistAsync(artist) as BadRequestResult;

            Assert.IsNotNull(retval);
            Assert.AreEqual(400, retval.StatusCode);
        }
    }
}
