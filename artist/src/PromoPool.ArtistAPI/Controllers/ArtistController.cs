using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PromoPool.ArtistAPI.Managers;
using PromoPool.ArtistAPI.Models;
using PromoPool.ArtistAPI.Services;
using System;
using System.IO;
using System.Net.Http.Headers;
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
        [Authorize("read:artists")]
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
        [Authorize]
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
        [Authorize]
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

        [HttpGet("{id}/logo")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Produces("application/json")]
        [Authorize]
        public IActionResult UploadLogoAsync(string id)
        {
            logger.LogInformation($"UploadLogo id: {id} - Resource Requested.");

            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    return Ok(new { dbPath });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }


        }

        
    }
}
