using BaatCheet.Features.AuthenticationFeatures.Commands;
using BaatCheet.Features.AuthenticationFeatures.Queries;
using BusinessLogicLayer.Model;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BaatCheet.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        /*private readonly IJwtAuthenticationManager jwtAuthenticationManager;*/
        private readonly IMediator mediator;
        public AuthenticationController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpGet]
        [Route("getUserDetails")]
        public async Task<ActionResult<object>> GetUserDetails([FromHeader] string authorization)
        {
            var response = await mediator.Send(new GetUserDetails.Query(authorization));
            if (response.UserDetails == null)
                return Unauthorized();
            return Ok(response.UserDetails);
            
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("registerUser")]
        public async Task<ActionResult> RegisterUser([FromBody] UserModel user)
        {
            var response = await mediator.Send(new RegisterUser.Query(user));
            if (response.Status == "User created")
                return Ok("User created sucessfully");
            return BadRequest("email is already available");
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("authenticateUser")]
        public async Task<ActionResult> AuthenticateUser(string email,string password)
        {
            var response = await mediator.Send(new AuthenticateUser.Query(email, password));
            if (response == null)
                return Unauthorized("Invalid email or password");
            return Ok(response.Token);
        }
    }
}
