using EpicCheckersBot.Checkers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicCheckersBot.Listener
{
    public class RequestBody
    {
        public Team Turn { get; set; }
        public Team?[][] Board { get; set;}
    }

}
