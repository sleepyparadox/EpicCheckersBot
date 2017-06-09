using EpicCheckersBot.Listener;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicCheckersBot
{
    class Program
    {
        static void Main(string[] args)
        {
            var listener = new CheckersListener();
            listener.Start();

            Console.WriteLine("Press enter to close");
            Console.ReadLine();
        }
    }
}
