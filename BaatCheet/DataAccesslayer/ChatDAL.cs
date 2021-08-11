using DataAccessLayer.Entities;
using DataAccessLayer.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class ChatDAL
    {
        private readonly BaatCheetDbContext dbContext;
        public ChatDAL()
        {
            this.dbContext = new BaatCheetDbContext();
        }
       
        public string GetConversationHash(int userContactId)
        {
            var query = this.dbContext.UserContacts.Where(x => x.Id == userContactId);
            foreach (var data in query)
            {
                return data.Name;
            }
            return "";
            /*var hash  =  new GetConversationHashById { Id = userContactId };
            return "";*/
        }
        public string GetGroupHash(int groupId)
        {
            var query = this.dbContext.Groups.Where(x => x.Id == groupId);
            foreach (var data in query)
            {
                return data.GroupHash;
            }
            return "";
        }

        public int GetUserId(int userContactId, int userId)
        {
            var query = this.dbContext.UserContacts.Where(x => x.Id == userContactId && (x.UserId == userId || x.ContactId == userId));
            foreach(var data in query)
            {
                if (data.UserId == userId)
                    return data.ContactId;
                else
                    return data.UserId;
            }
            return -1;
        }

        public List<int> GetAllUsersInGroup(int groupId)
        {
            var query = this.dbContext.UserGroups.Where(x => x.GroupId == groupId);
            List<int> userId = new List<int>();
            foreach(var data in query)
            {
                userId.Add(data.UserId);
            }
            return userId;
        }

        public bool SaveConversation(Conversation conversation)
        {
            try
            {
                this.dbContext.Add(conversation);
                this.dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            
        }

        public bool SaveGroupConversation(GroupConversation groupConversation)
        {
            try
            {
                this.dbContext.Add(groupConversation);
                this.dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
