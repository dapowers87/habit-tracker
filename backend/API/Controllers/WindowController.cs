using System.Net;
using System.Threading.Tasks;
using Application.Actions.Account;
using Application.Actions.Window;
using Domain.ApiModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Persistence.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]/[Action]")]
    public class WindowController : ControllerBase
    {
        private readonly ILogger<WindowController> _logger;
        private readonly IMediator mediator;

        public WindowController(ILogger<WindowController> logger, IMediator mediator)
        {
            _logger = logger;
            this.mediator = mediator;
        }

        [HttpPost]
        [Authorize]
        [SwaggerResponse((int)HttpStatusCode.Created)]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<int>> Create(CreateWindowModel model)
        {
            var result = await mediator.Send(new Create.Command { Model = model });

            return result == -1 ? BadRequest(-1) : Created($"ap1/v1/Window/Get?windowId={result}", result);
        }

        [HttpGet]
        [Authorize]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
        public async Task<ActionResult<Window>> Get(int windowId)
        {
            var result = await mediator.Send(new Get.Query { WindowId = windowId });

            return result == null ? NotFound() : Ok(result);
        }

        [HttpPut]
        [Authorize]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
        [SwaggerResponse((int)HttpStatusCode.NoContent)]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        public async Task<ActionResult> Update(UpdateWindowModel model)
        {
            var result = await mediator.Send(new Update.Command { Model = model });

            switch(result)
            {
                case -1:
                    return NotFound();
                case 0:
                    return BadRequest();
                case 1:
                    return NoContent();
                default:
                    return Ok();
            }
        }

        [HttpPost]
        [Authorize]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
        [SwaggerResponse((int)HttpStatusCode.NoContent)]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        public async Task<ActionResult> Update(int windowId, int newCheatDaysUsed)
        {
            var result = await mediator.Send(new UpdateCheatDays.Command { WindowId = windowId, NewCheatDaysUsed = newCheatDaysUsed });

            switch(result)
            {
                case -1:
                    return NotFound();
                case 0:
                    return BadRequest();
                case 1:
                    return NoContent();
                default:
                    return Ok();
            }
        }

        [HttpDelete]
        [Authorize]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        public async Task<ActionResult> Delete(int windowId, int newCheatDaysUsed)
        {
            var result = await mediator.Send(new Delete.Command { WindowId = windowId });

            switch(result)
            {
                case -1:
                    return NotFound();
                case 0:
                    return BadRequest();
                case 1:
                    return Ok();
                default:
                    return Ok();
            }
        }
    }
}
