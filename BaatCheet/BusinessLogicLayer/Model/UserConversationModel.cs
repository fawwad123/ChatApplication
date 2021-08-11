using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Model
{
    public class UserConversationModel
    {
        public UserConversationModel()
        {
            Person = new PersonModel();
        }
        public int UserContactId { get; set; }
        public PersonModel Person { get; set; }
        public ICollection<ConversationModel> Messages { get; set; }
    }
}
