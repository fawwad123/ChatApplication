using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Model
{
    public class GroupChatModel
    {
        public GroupChatModel()
        {
            CreatedBy = new PersonModel();
            GroupMember = new List<PersonModel>();
            Chats = new List<ChatModal>();
        }
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public bool? IsAdmin { get; set; }
        public PersonModel CreatedBy { get; set; }
        public List<PersonModel> GroupMember { get; set; }
        public List<ChatModal> Chats { get; set; }
    }
}
