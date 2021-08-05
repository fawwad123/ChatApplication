using BusinessLogicLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaatCheet.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserBLL userBLL;
        public UserController()
        {
            this.userBLL = new UserBLL();
        }
        [HttpPost]
        [Route("addNewContact")]
        public ActionResult AddNewContact(string email, string userId, [FromHeader] string authorization)
        {
            var message = "";
            object user = userBLL.AddNewContact(email, userId, authorization, ref message);

            if (message == "UnAuthorized user")
                return Unauthorized(message);
            else if (message == "User added successfully")
                return Ok(user);
            else
                return Ok(message);
        }
        [HttpPost]
        [Route("addNewGroup")]
        public ActionResult AddNewGroup(string groupName, string userId, [FromHeader] string authorization)
        {
            var message = "";
            object user = userBLL.AddNewGroup(groupName, userId, authorization, ref message);

            if (message == "UnAuthorized user")
                return Unauthorized(message);
            return Ok(user);
        }

        [HttpPost]
        [Route("addUserToGroup")]
        public ActionResult AddUserToGroup(string email, string userId, string groupId, [FromHeader] string authorization)
        {
            var message = "";
            userBLL.AddUserToGroup(email, userId, groupId, authorization, ref message);

            if (message == "UnAuthorized user")
                return Unauthorized(message);
            return Ok(message);
        }
    }
}
