using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstSoap
{
    class Program
    {
        static void Main(string[] args)
        {
            mathOp.MathsOperationsClient calc = new mathOp.MathsOperationsClient();
            int result = calc.Add(2, 3);
            Console.WriteLine(result);
            Console.ReadLine();
        }
    }
}
