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


        

    }
}
