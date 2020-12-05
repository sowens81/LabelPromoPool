using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PromoPool.ArtistAPI.Managers;
using PromoPool.ArtistAPI.Models;
using PromoPool.ArtistAPI.Services;
using System;
using System.Collections.Generic;
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
        [ProducesResponseType(typeof(IEnumerable<Artist>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Produces("application/json")]
        // [Authorize("read:artists")]
        public async Task<IActionResult> GetArtistsAsync(string artistname)
        {
            logger.LogInformation($"GetArtists - Resource Requested.");

            if (artistname != null)
            {
                var artistsSearchByName = await artistManager.FindAllArtistsByNameAsync(artistname);

                if (artistsSearchByName != null)
                {
                    return Ok(artistsSearchByName);
                }

                return NotFound();
            }

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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Produces("application/json")]
        // [Authorize]
        public async Task<IActionResult> GetArtistByIdAsync(string id)
        {
            logger.LogInformation($"GetArtist id: {id} - Resource Requested.");

            var validateId = validation.ValidateId(id);


            if (validateId.resultValid)
            {
                var artist = await artistManager.GetArtistByIdAsync(id);

                if (artist != null)
                {
                    return Ok(artist);
                }

                return NotFound();
  
            }

            return BadRequest(validateId.message);

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

            var validateArtist = validation.ValidateNewArtistModel(newArtist);

            if (validateArtist.resultValid)
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
            return BadRequest(validateArtist.message);
        }

        [ApiVersion("1.0")]
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<IActionResult> UpdateArtistByIdAsync(string id, UpdateArtist updateArtist)
        {
            logger.LogInformation($"Update Artist Body: {updateArtist} - Resource Requested.");

            var validateId = validation.ValidateId(id);


            if (validateId.resultValid)
            {
                var validateArtist = validation.ValidateUpdateArtistModel(updateArtist);

                if (validateArtist.resultValid)
                {
                    if (ModelState.IsValid)
                    {
                        var updatedArtist = await artistManager.UpdateArtistAsync(id, updateArtist);

                        if (updatedArtist != null)
                        {
                            return Ok(updatedArtist);
                        }

                        return NotFound();
                    }
                    else
                    {
                        return BadRequest(ModelState);
                    }
                }

                return BadRequest(validateArtist.message);
            }

            return BadRequest(validateId.message);


        }

        [ApiVersion("1.0")]
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Produces("application/json")]
        // [Authorize]
        public async Task<IActionResult> DeleteArtistByIdAsync(string id)
        {
            logger.LogInformation($"DeleteArtist id: {id} - Resource Requested.");

            var validateId = validation.ValidateId(id);

            if (validateId.resultValid)
            {
                var deleteSuccessful = await artistManager.DeleteArtistByIdAsync(id);

                if (deleteSuccessful)
                {
                    return Accepted();
                }

                return NotFound();
            }
            return BadRequest(validateId.message);

        }

        [ApiVersion("1.0")]
        [HttpDelete]
        [ProducesResponseType(typeof(string), StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Produces("application/json")]
        // [Authorize]
        public async Task<IActionResult> DeleteAllArtistsAsync()
        {
            logger.LogInformation($"DeleteAllArtists - Resource Requested.");



                var deleteSuccessful = await artistManager.DeleteAllArtistsAsync();

                if (deleteSuccessful)
                {
                    return Accepted();
                }

                return NotFound();

        }

    }
}
