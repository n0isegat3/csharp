using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppArguments
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("App started {0}", DateTime.Now);

            // Display the number of command line arguments.
            Console.WriteLine("Number of arguments: {0}",args.Length);

            if (args.Length != 3)
            {
                Console.WriteLine("Help: AppArguments.exe [firstArg] [secondArg] [thirdArg]");
            }
            else
            {

                switch (args[0])
                {
                    case "connectShare":
                        Console.WriteLine("[>] option: connectShare");
                        break;
                    case "authenticate":
                        Console.WriteLine("[>] option: authenticate");
                        break;
                    default:
                        Console.WriteLine("[>] option: unknown");
                        break;
                }
            }

            Console.WriteLine("App finished {0}", DateTime.Now);
        }
    }
}
