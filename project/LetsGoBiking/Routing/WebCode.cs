using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Routing
{
    public enum WebCode: int
    {
        WITHBIKE = 200,
        WITHOUTBIKE = 201,
        NOSTATION = 202,
        BADADDRESS = 203
    }
}
