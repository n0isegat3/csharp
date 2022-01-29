using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace n0iseEncoderCaesar
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //payload sample not complete follows
            byte[] buf = new byte[5] { 0x01, 0x10, 0x83, 0xe4, 0xf0 };

            Int32 caesarSubstitionKey = 5; //change the substition key here!
            //String Usage = "csharp";
            String Usage = "vba";

            byte[] encoded = new byte[buf.Length];
            for (int i = 0; i < buf.Length; i++)
            {
                encoded[i] = (byte)(((uint)buf[i] + caesarSubstitionKey) & 0xFF);
            }

            StringBuilder hex = new StringBuilder(encoded.Length * 2);

            if (Usage == "csharp")
            {
                foreach (byte b in encoded)
                {
                    hex.AppendFormat("0x{0:x2}, ", b);
                }

                Console.WriteLine("Ceasar encoded payload for csharp:");
                Console.WriteLine("byte[] buf = new byte[{0}] {1}{2}{3}", buf.Length, "{", hex.ToString().TrimEnd(' ').TrimEnd(','), "};");
                Console.WriteLine();
                Console.WriteLine("Decode snippet:");
                Console.WriteLine("for(int e = 0; e < buf.Length; e++)");
                Console.WriteLine("{");
                Console.WriteLine("\t" + @"buf[e] = (byte)(((uint)buf[e] - " + caesarSubstitionKey + @") & 0xFF);");
                Console.WriteLine("}");
            }

            if (Usage == "vba")
            {
                uint counter = 0;
                foreach (byte b in encoded)
                {
                    hex.AppendFormat("{0:D}, ", b);
                    counter++;
                    if (counter % 50 == 0)
                    {
                        hex.AppendFormat("_{0}", Environment.NewLine);
                    }
                }

                Console.WriteLine("Ceasar encoded payload for vba:");
                Console.WriteLine("buf = Array{1}{2}{3}", buf.Length, "(", hex.ToString().TrimEnd(' ').TrimEnd(','), ")");
                Console.WriteLine();
                Console.WriteLine("Decode snippet:");
                Console.WriteLine("for i = 0 To Ubound(buf)");
                Console.WriteLine("\t" + @"buf(i) = buf(i) - " + caesarSubstitionKey);
                Console.WriteLine("Next i");
            }
        }
    }
}