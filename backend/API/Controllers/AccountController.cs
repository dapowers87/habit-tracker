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
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
        public async Task<ActionResult<string>> Login(string username, string password)
        {
            var ipAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();

            var result = await mediator.Send(new Login.Command { IpAddress = ipAddress, Username = username, Password = password });

            if(string.IsNullOrEmpty(result))
            {
                if(result == null) // Locked Out
                {
                    return Unauthorized("Locked out");
                }
                else
                {
                    return Unauthorized("Invalid Password");
                }
            }

            return Ok(result);
        }

        [HttpGet]
        [Authorize]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
        public ActionResult QuickAuthorizationCheck()
        {
            return Ok();
        }
    }
}
