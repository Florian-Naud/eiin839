using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Routing
{
    public class WebResult
    {
        public int code { get; set; }
        public Route result { get; set; }

        public WebResult()
        {
        }

        public WebResult(int code, Route res)
        {
            this.code = code;
            this.result = res;
        }
    }
}
