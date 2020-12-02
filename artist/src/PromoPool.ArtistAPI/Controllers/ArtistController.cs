using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PromoPool.ArtistAPI.Managers;
using PromoPool.ArtistAPI.Models;
using PromoPool.ArtistAPI.Services;
using System.Threading.Tasks;

namespace PromoPool.ArtistAPI.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{v:apiVersion}/artist")]
    [ApiController]

    public class ArtistController : ControllerBase
    {
        private readonly IArtistManager artistManager;
        private readonly ILogger logger;
        private readonly IValidation validation;

        public ArtistController(IArtistManager artistManager,ILogger<ArtistController> logger, IValidation validation)
        {
            this.artistManager = artistManager;
            this.logger = logger;
            this.validation = validation;
        }


        [ApiVersion("1.0")]
        [HttpGet]
        [ProducesResponseType(typeof(Artist), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Produces("application/json")]
        // [Authorize("read:artists")]
        public async Task<IActionResult> GetArtistsAsync()
        {
            logger.LogInformation($"GetArtists - Resource Requested.");

            var artists = await artistManager.GetAllArtistsAsync();

            if (artists != null)
            {
                return Ok(artists);
            }

            return NotFound();
  
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Artist), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Produces("application/json")]
        // [Authorize]
        public async Task<IActionResult> GetArtistByIdAsync(string id)
        {
            logger.LogInformation($"GetArtist id: {id} - Resource Requested.");

            
            if (validation.ValidateId(id))
            {
                var artist = await artistManager.GetArtistByIdAsync(id);

                if (artist != null)
                {
                    return Ok(artist);
                }
  
            }

            return NotFound();

        }

        [ApiVersion("1.0")]
        [HttpPost]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Produces("application/json")]
        // [Authorize]
        public async Task<IActionResult> AddArtistAsync(NewArtist newArtist)
        {
            logger.LogInformation($"AddArtist Body: {newArtist} - Resource Requested.");

            if (validation.ValidateNewArtistModel(newArtist))
            {
                if (ModelState.IsValid)
                {
                    var id = await artistManager.InsertArtistAsync(newArtist);

                    if (id != null)
                    {
                        return Created(id, id);
                    }
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            return BadRequest();
        }

        [ApiVersion("1.0")]
        [HttpDelete]
        [ProducesResponseType(typeof(string), StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Produces("application/json")]
        // [Authorize]
        public async Task<IActionResult> DeleteArtistByIdAsync(string id)
        {
            logger.LogInformation($"DeleteArtist id: {id} - Resource Requested.");


            if (validation.ValidateId(id))
            {
                var artist = await artistManager.DeleteArtistByIdAsync(id);

                if (artist == true)
                {
                    return Accepted();
                }

            }

            return BadRequest();

        }

    }
}
