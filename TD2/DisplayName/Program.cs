using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisplayName
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 2)
                Console.WriteLine("<HTML><BODY> Hello " + args[0] + " et " + args[1] + "</BODY></HTML>");
            else
                Console.WriteLine("We need two arguments");
        }
    }
}
