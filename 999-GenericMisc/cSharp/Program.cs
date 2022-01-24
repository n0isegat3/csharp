using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cSharp
{
    internal class Program
    {
        private static Random random = new Random();

        public static string RandomString(int length)
        {
            //const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            //const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string chars = "ABCDEFGHJKLMNPRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

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

            Console.WriteLine(RandomString(8));

            Console.ReadKey();


        }
    }
}
