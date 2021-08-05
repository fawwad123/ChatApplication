using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Model
{
    public class ConversationModel
    {
        public int UserContactId { get; set; }
        public string Message { get; set; }
        public int MessageBy { get; set; }
        public DateTime MessageOn { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
