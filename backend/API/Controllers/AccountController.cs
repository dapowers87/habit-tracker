using System.Net;
using System.Threading.Tasks;
using Application.Actions.Account;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]/[Action]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IMediator mediator;

        public AccountController(ILogger<AccountController> logger, IMediator mediator)
        {
            _logger = logger;
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<string>> Login(string password)
        {
            var ip = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            var result = await mediator.Send(new Login.Command { IpAddress = ip, Password = password });

            return string.IsNullOrEmpty(result) ? NotFound() : Ok(result);
        }

        [HttpGet]
        [Authorize]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
        public ActionResult QuickAuthorizationCheckRaptor()
        {
            return Ok();
        }
    }
}
