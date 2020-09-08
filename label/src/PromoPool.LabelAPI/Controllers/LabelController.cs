using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PromoPool.LabelAPI.Exceptions;
using PromoPool.LabelAPI.Managers;
using PromoPool.LabelAPI.Models;
using System;
using System.Threading.Tasks;

namespace PromoPool.LabelAPI.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{v:apiVersion}/labels")]
    [ApiController]

    public class LabelController : ControllerBase
    {
        private readonly ILabelManager labelManager;
        private readonly ILogger logger;

        public LabelController(ILabelManager labelManager,ILogger<LabelController> logger)
        {
            this.labelManager = labelManager;
            this.logger = logger;

        }

        [ApiVersion("1.0")]
        [HttpGet]
        [HttpGet]
        [ProducesResponseType(typeof(Label), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Produces("application/json")]
        [Authorize]
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

            if (string.IsNullOrEmpty(id))
            {
                throw new MissingIdException("Id is null or empty", nameof(id));
            }

            if (!Guid.TryParse(id, out _))
            {
                throw new MismatchIdException("Id is not in a Guid format", nameof(id));
            }

            var label = await labelManager.GetLabelByIdAsync(id);

            if (label == null)
            {
                return NotFound();
            }

            return Ok(label);
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
            if (newLabel == null)
            {
                throw new ArgumentException("No label", nameof(newLabel));
            }

            logger.LogInformation($"AddLabel Body: {newLabel} - Resource Requested.");
            if(ModelState.IsValid)
            {
                var id = await labelManager.InsertLabelAsync(newLabel);

                if (id == null)
                {
                    return BadRequest();
                }

                return Created(id, id);
            }
            else
            {
                return BadRequest(ModelState);
            }   
        }

        
    }
}
