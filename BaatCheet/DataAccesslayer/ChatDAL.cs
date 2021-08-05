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
        public void SaveConversation(Conversation conversation)
        {
            try
            {
                this.dbContext.Add(conversation);
                this.dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }

        public void SaveGroupConversation(GroupConversation groupConversation)
        {
            try
            {
                this.dbContext.Add(groupConversation);
                this.dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
