
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Common
{
    public static class Commons
    {
        public static ulong Hash()
        {
            ulong kind = (ulong)(int)DateTime.Now.Kind;
            return (kind << 62) | (ulong)DateTime.Now.Ticks;
        }
    }
}
