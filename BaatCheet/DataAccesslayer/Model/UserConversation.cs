using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Model
{
    public class UserConversation
    {
        public UserConversation()
        {
            Person = new Person();
        }
        public int UserContactId { get; set; }
        public Person Person { get; set; }
        public ICollection<Conversation> Messages { get; set; }
    }
}
