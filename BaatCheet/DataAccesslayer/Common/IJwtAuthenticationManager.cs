using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLayer.Common
{
    public interface IJwtAuthenticationManager
    {
        string Authenticate(string username, string password);
    }
}
