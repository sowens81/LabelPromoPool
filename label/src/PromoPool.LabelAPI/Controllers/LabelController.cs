using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PromoPool.LabelAPI.Managers;
using PromoPool.LabelAPI.Models;
using PromoPool.LabelAPI.Services;
using System;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace PromoPool.LabelAPI.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{v:apiVersion}/label")]
    [ApiController]

    public class LabelController : ControllerBase
    {
        private readonly ILabelManager labelManager;
        private readonly ILogger logger;
        private readonly IValidation validation;

        public LabelController(ILabelManager labelManager,ILogger<LabelController> logger, IValidation validation)
        {
            this.labelManager = labelManager;
            this.logger = logger;
            this.validation = validation;
        }


        [ApiVersion("1.0")]
        [HttpGet]
        [ProducesResponseType(typeof(Label), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Produces("application/json")]
        [Authorize("read:labels")]
        public async Task<IActionResult> GetLabelsAsync()
        {
            logger.LogInformation($"GetLabels - Resource Requested.");

            var labels = await labelManager.GetAllLabelsAsync();

            if (labels != null)
            {
                return Ok(labels);
            }

            return NotFound();
  
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Label), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Produces("application/json")]
        [Authorize]
        public async Task<IActionResult> GetLabelByIdAsync(string id)
        {
            logger.LogInformation($"GetLabel id: {id} - Resource Requested.");

            
            if (validation.ValidateId(id))
            {
                var label = await labelManager.GetLabelByIdAsync(id);

                if (label != null)
                {
                    return Ok(label);
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
        public async Task<IActionResult> AddLabelAsync(NewLabel newLabel)
        {
            logger.LogInformation($"AddLabel Body: {newLabel} - Resource Requested.");

            if (validation.ValidateNewLabelModel(newLabel))
            {
                if (ModelState.IsValid)
                {
                    var id = await labelManager.InsertLabelAsync(newLabel);

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
