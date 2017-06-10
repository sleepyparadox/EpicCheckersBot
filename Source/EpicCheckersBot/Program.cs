using EpicCheckersBot.Checkers;
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
            Game.RunPracticeGame();

            //var listener = new CheckersListener();
            //listener.Start();

            //var js = CheckersListener.GetResponseJavascript("eyJUdXJuIjoiUmVkIiwiQm9hcmQiOltbIlJlZCIsIlJlZCIsIlJlZCIsIlJlZCIsIlJlZCIsIlJlZCIsIlJlZCIsIlJlZCJdLFtudWxsLG51bGwsbnVsbCxudWxsLG51bGwsbnVsbCxudWxsLG51bGxdLFtudWxsLG51bGwsbnVsbCxudWxsLG51bGwsbnVsbCxudWxsLG51bGxdLFtudWxsLG51bGwsbnVsbCxudWxsLG51bGwsbnVsbCxudWxsLG51bGxdLFtudWxsLG51bGwsbnVsbCxudWxsLG51bGwsbnVsbCxudWxsLG51bGxdLFtudWxsLG51bGwsbnVsbCxudWxsLG51bGwsbnVsbCxudWxsLG51bGxdLFtudWxsLCJCbHVlIixudWxsLG51bGwsbnVsbCxudWxsLG51bGwsbnVsbF0sWyJCbHVlIixudWxsLCJCbHVlIiwiQmx1ZSIsIkJsdWUiLCJCbHVlIiwiQmx1ZSIsIkJsdWUiXV19");

            Console.WriteLine("Press enter to close");
            Console.ReadLine();
        }
    }
}
