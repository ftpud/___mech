using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Mech.Server;

namespace MECH_SIM
{
    


    /// <summary>
    /// Main
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Mech fight Test Server!");


            Server s = new Server();
            s.Start();

            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}
