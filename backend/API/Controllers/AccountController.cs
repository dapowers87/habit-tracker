using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Application.Actions.Account;
using Domain.ApiModels;
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
                    return Unauthorized("Invalid Username/Password");
                }
            }

            return Ok(result);
        }

        [HttpPost]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<string>> CreateUser(string username, string password)
        {
            var result = await mediator.Send(new CreateUser.Command 
            { 
                Username = username, 
                Password = password,
                IsAdmin = false
            });

            if(!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(result);
        }

        [HttpPost]
        [Authorize]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<string>> CreateAdminUser(string username, string password)
        {
            var result = await mediator.Send(new CreateUser.Command 
            { 
                Username = username, 
                Password = password,
                IsAdmin = true
            });

            if(!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(result);
        }

        [HttpGet]
        [Authorize]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        public async Task<List<string>> GetUsernames()
        {
            var result = await mediator.Send(new GetUsernames.Query());

            return result;
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
