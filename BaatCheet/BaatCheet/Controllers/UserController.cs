using BaatCheet.Features.ChatFeatures.Commands;
using BusinessLogicLayer;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BaatCheet.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserBLL userBLL;
        private readonly IMediator mediator;
        public UserController(IMediator mediator)
        {
            this.mediator = mediator;
            this.userBLL = new UserBLL();
        }

        /*[HttpPost]
        [Route("addNewContact")]
        public async Task<ActionResult> AddNewContact(string email, int userId, [FromHeader] string authorization)
        {
            var response = await mediator.Send(new AddNewContact.Query(email, userId, authorization));
            if (response.Message == "UnAuthorized user")
                return Unauthorized(response.Message);
            else if (response.Message == "User added successfully")
                return Ok(response.User);
            else
                return Ok(response.Message);
        }*/
        /*[HttpPost]
        [Route("addNewGroup")]
        public async Task<ActionResult> AddNewGroup(string groupName, string userId, [FromHeader] string authorization)
        {
            var response = await mediator.Send(new AddNewGroup.Query(groupName, userId, authorization));
            if (response.Message == "UnAuthorized user")
                return Unauthorized(response.Message);
            return Ok(response.User);
        }*/

        /*[HttpPost]
        [Route("addUserToGroup")]
        public async Task<ActionResult> AddUserToGroup(string email, string userId, string groupId, [FromHeader] string authorization)
        {
            var response = await mediator.Send(new AddUserToGroup.Query(email, userId, groupId, authorization));

            if (response.Message == "UnAuthorized user")
                return Unauthorized(response.Message);
            return Ok(response.Message);
        }*/
    }
}
