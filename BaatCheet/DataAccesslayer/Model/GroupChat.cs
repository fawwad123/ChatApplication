using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Model
{
    public class GroupChat
    {
        public GroupChat()
        {
            CreatedBy = new Person();
            GroupMember = new List<Person>();
            Chats = new List<Chat>();
        }
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string Name { get; set; }
        public bool? IsAdmin { get; set; }
        public Person CreatedBy { get; set; }
        public List<Person> GroupMember { get; set; }
        public List<Chat> Chats { get; set; }
    }
}
