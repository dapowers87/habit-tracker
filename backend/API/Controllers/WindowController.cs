using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
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

        private string GetUsername()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var username = string.Empty;
            if (identity != null)
            {
                username = identity.FindFirst("username")?.Value;
            }

            return username;
        }

        [HttpPost]
        [Authorize]
        [SwaggerResponse((int)HttpStatusCode.Created)]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<int>> Create(CreateWindowModel model)
        {
            var result = await mediator.Send(new Create.Command { Username = GetUsername(), Model = model });

            return (result == -1 || result == 2) ? BadRequest(result) : Created($"ap1/v1/Window/Get?windowId={result}", result);
        }

        [HttpGet]
        [Authorize]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
        public async Task<ActionResult<WindowWithoutUser>> Get(int windowId)
        {
            var result = await mediator.Send(new Get.Query { Username = GetUsername(), WindowId = windowId });

            return result == null ? NotFound() : Ok(result);
        }

        [HttpGet]
        [Authorize]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
        public async Task<ActionResult<List<WindowWithoutUser>>> GetAll(bool includeExpired)
        {
            var result = await mediator.Send(new GetAll.Query { Username = GetUsername(), IncludeExpired = includeExpired });

            return Ok(result);
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
            var result = await mediator.Send(new Update.Command { Username = GetUsername(), Model = model });

            switch(result)
            {
                case 2:    
                case -1:
                    return NotFound();
                case 1:
                    return Ok(true);
                default:
                    return BadRequest("Unknown Error");
            }
        }

        [HttpPost]
        [Authorize]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
        [SwaggerResponse((int)HttpStatusCode.NoContent)]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        public async Task<ActionResult> UpdateCheatDays(int windowId, int newCheatDaysUsed)
        {
            var result = await mediator.Send(new UpdateCheatDays.Command { Username = GetUsername(), WindowId = windowId, NewCheatDaysUsed = newCheatDaysUsed });

            switch(result)
            {
                case 2:
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
        public async Task<ActionResult> Delete(int windowId)
        {
            var result = await mediator.Send(new Delete.Command { Username = GetUsername(), WindowId = windowId });

            switch(result)
            {
                case 2:
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
