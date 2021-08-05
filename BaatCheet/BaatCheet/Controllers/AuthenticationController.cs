using BusinessLogicLayer;
using BusinessLogicLayer.Model;
using DataAccessLayer.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaatCheet.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly AuthenticationBLL authenticationBLL;
        private readonly IJwtAuthenticationManager jwtAuthenticationManager;
        public AuthenticationController(IJwtAuthenticationManager jwtAuthenticationManager)
        {
            this.authenticationBLL = new AuthenticationBLL();
            this.jwtAuthenticationManager = jwtAuthenticationManager;
        }
        [HttpGet]
        [Route("getUserDetails")]
        public ActionResult<object> GetUserDetails([FromHeader] string authorization)
        {
            string message = "";
            object userDetails = authenticationBLL.GetUserDetails(authorization, ref message);
            if (userDetails != null)
                return Ok(userDetails);
            return Unauthorized();
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("registerUser")]
        public ActionResult RegisterUser([FromBody] UserModel user)
        {
            string status =  authenticationBLL.RegisterUser(user);
            if (status == "User created")
                return Ok("User created sucessfully");
            return BadRequest("email is already available");
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("authenticateUser")]
        public ActionResult AuthenticateUser(string email,string password)
        {
            var token =  authenticationBLL.AuthenticateUser(email, password);
            if (token == null)
                return Unauthorized("Invalid email or password");
            return Ok(token);
        }
    }
}
