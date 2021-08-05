using BusinessLogicLayer.Model;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class UserBLL
    {
        private readonly UserDAL userDAL;
        public UserBLL()
        {
            userDAL = new UserDAL();
        }

        public object AddNewContact(string email, string userId, string token, ref string message)
        {
            return userDAL.AddNewContact(email, userId, token, ref message);
        }

        public object AddNewGroup(string groupName, string userId, string token, ref string message)
        {
            return userDAL.AddNewGroup(groupName, userId, token, ref message);
        }

        public void AddUserToGroup(string email, string userId, string groupId, string token, ref string message)
        {
            userDAL.AddUserToGroup(email, userId, groupId, token, ref message);
        }

        public string GetUserContactEmail(int userContactId, int messageBy)
        {
            return userDAL.GetUserContactEmail(userContactId, messageBy);
        }
    }
}
