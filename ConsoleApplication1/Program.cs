using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {

            EFData.Class1 c = new EFData.Class1();

            string s = c.Getsss();
               
            Console.WriteLine(s);

            Console.Read();
        }
    }
}
