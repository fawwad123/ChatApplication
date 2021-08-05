using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Model
{
    public class UserInfo
    {
        public UserInfo()
        {
            Person = new Person();
        }
        public Person Person { get; set; } 
        public object ChatDetails { get; set; }
        public object GroupDetails { get; set; }
    }
}
