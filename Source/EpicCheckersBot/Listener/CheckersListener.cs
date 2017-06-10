using EpicCheckersBot.Checkers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EpicCheckersBot.Listener
{
    public class CheckersListener
    {
        private HttpListener _httpListener;

        public void Start()
        {
            _httpListener = new HttpListener();
            _httpListener.Prefixes.Add("http://localhost/");
            _httpListener.Start();

            _httpListener.BeginGetContext(StartRequestProcess, null);
        }

        private void StartRequestProcess(IAsyncResult ar)
        {
            HttpListenerContext context;
            context = _httpListener.EndGetContext(ar);

            // Bargain url parsing
            string responseJavascript;
            if (context.Request.RawUrl.Contains("base64="))
            {
                var argsBase64 = context.Request.RawUrl.Split(new[] { "base64=" }, StringSplitOptions.None)[1];
                responseJavascript = GetResponseJavascript(argsBase64);
            }
            else
            {
                responseJavascript = "console.log(\"Missing base64 argument\")";
            }

            using (var outStream = new StreamWriter(context.Response.OutputStream))
            {
                outStream.Write(responseJavascript);
            }

            _httpListener.BeginGetContext(StartRequestProcess, null);
        }

        public static string GetResponseJavascript(string argsBase64)
        {
            var argsBytes = Convert.FromBase64String(argsBase64);
            var argsJson = Encoding.ASCII.GetString(argsBytes);

            var args = JsonConvert.DeserializeObject<RequestBody>(argsJson);

            var game = new Game(new Board(args.Round, args.Board));
            Renderer.Renderer.RenderToConsole(game.Board);

            var bestMove = game.GetBestMove(args.Turn);

            string javascript;
            if(bestMove.HasValue)
            {
                var from = bestMove.Value.From;
                var to = bestMove.Value.To;
                javascript = string.Format("BootLoader.MoveUnit({0}, {1}, {2}, {3});", from.row, from.col, to.row, to.col);
            }
            else
            {
                javascript = "console.log(\"couldn't find a move sorry\")";
            }

            Console.WriteLine("Response:");
            Console.WriteLine(javascript);

            // Return best move ... as javascript
            return javascript;
        }
    }
}
