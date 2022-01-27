using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace n0iseEncoderXOR
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //payload sample not complete follows
            byte[] buf = new byte[5] { 0xfc, 0x48, 0x83, 0xe4, 0xf0 };

            byte[] encoded = new byte[buf.Length];
            for (int i = 0; i < buf.Length; i++)
            {
                encoded[i] = (byte)((uint)buf[i] ^ 0xfe); //change the XOR fixed key here and below!
            }

            StringBuilder hex = new StringBuilder(encoded.Length * 2);
            foreach (byte b in encoded)
            {
                hex.AppendFormat("0x{0:x2}, ", b);
            }

            Console.WriteLine("XOR encoded payload:", buf.Length, hex.ToString());
            Console.WriteLine("byte[] buf = new byte[{0}] {1}{2}{3}", buf.Length, "{", hex.ToString().TrimEnd(' ').TrimEnd(','), "};");
            Console.WriteLine();
            Console.WriteLine("Decode snippet:");
            Console.WriteLine("for(int e = 0; e < buf.Length; e++)");
            Console.WriteLine("{");
            Console.WriteLine("\t" + @"buf[e] = (byte)((uint)buf[e] ^ " + "0xfe" + @");"); //change the XOR fixed key here and up!
            Console.WriteLine("}");
        }
    }
}
