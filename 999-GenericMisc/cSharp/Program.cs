using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cSharp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            String var1 = "Value1";
            Console.WriteLine(var1);

            String text = String.Format("var1 value is {0}", var1);
            Console.WriteLine(text);

            String UNC = "\\server\\share";
            Console.WriteLine(UNC);

            if (UNC.StartsWith("\\\\") == true)
            {
                Console.WriteLine("smb path is correct");
            }

            Console.ReadKey();
        }
    }
}
