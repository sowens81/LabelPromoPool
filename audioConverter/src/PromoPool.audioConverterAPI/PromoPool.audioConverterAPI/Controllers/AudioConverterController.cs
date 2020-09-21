using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PromoPool.audioConverterAPI.Models;
using PromoPool.audioConverterAPI.Services;

namespace PromoPool.audioConverterAPI.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{v:apiVersion}/apiconverter")]
    [ApiController]
    public class AudioConverterController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly IMessageQueue messageQueue;

        public AudioConverterController(ILogger<AudioConverterController> logger, IMessageQueue messageQueue)
        {
            this.logger = logger;
            this.messageQueue = messageQueue;
        }

        [ApiVersion("1.0")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Produces("application/json")]
        public IActionResult PostAudioFile(AudioSchema audioSchema)
        {
            logger.LogInformation($"PostAudioFile Body: {audioSchema} - Resource Requested.");

                if (ModelState.IsValid)
                {
                    if (messageQueue.AddQueueMessage(audioSchema) == "Sent")
                    {
                        return Accepted();
                    }
                }

            return BadRequest();

        }

    }
}
