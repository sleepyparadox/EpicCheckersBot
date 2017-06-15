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
            _httpListener.Prefixes.Add("http://*:80/");
            _httpListener.Start();

            _httpListener.BeginGetContext(StartRequestProcess, null);
        }

        private void StartRequestProcess(IAsyncResult ar)
        {
            HttpListenerContext context;
            context = _httpListener.EndGetContext(ar);

            //try
            {
                Console.WriteLine("Request: " + context.Request.HttpMethod + " " + context.Request.RawUrl);

                // Bargain url parsing
                string requestJson;
                using (var inStream = new StreamReader(context.Request.InputStream))
                {
                    requestJson = inStream.ReadToEnd();
                }

                string responseBody;
                if(context.Request.HttpMethod == "POST")
                {
                    var requestParsed = JsonConvert.DeserializeObject<RequestBody>(requestJson);
                    var nextMove = GetNextMove(requestParsed);
                    responseBody = JsonConvert.SerializeObject(nextMove);

                    Console.WriteLine("Response:" + responseBody);
                }
                else if(context.Request.HttpMethod == "GET")
                {
                    responseBody = File.ReadAllText("BootLoader.js");
                    Console.WriteLine("Response: BootLoader.js");
                }
                else
                {
                    responseBody = string.Empty;
                    Console.WriteLine("Response:");
                }

                // Set all these headers so browers at http://epiccheckers.appspot.com/ can connect here
                context.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Accept, X-Requested-With");
                context.Response.AddHeader("Access-Control-Allow-Methods", "GET, POST");
                context.Response.AddHeader("Access-Control-Max-Age", "1728000");
                context.Response.AppendHeader("Access-Control-Allow-Origin", "*");
                context.Response.StatusCode = 200;

                using (var outStream = new StreamWriter(context.Response.OutputStream))
                {
                    outStream.Write(responseBody);
                }
            }
            //catch(Exception e)
            {
                //Console.Clear();
                //Console.WriteLine("Bad request");
                //Console.WriteLine(e.ToString());
            }

            _httpListener.BeginGetContext(StartRequestProcess, null);
        }

        public static object GetNextMove(RequestBody request)
        {
            var game = new Game(new Board(request.Round, request.Board));
            Renderer.Renderer.RenderToConsole(game.Board);
            Console.WriteLine("Calculating");

            var bestMove = game.GetBestMove(request.Turn);
            
            if(bestMove.HasValue)
            {
                game.Board.PushMove(bestMove.Value);

                // render result
                game.Board.GetScore(request.Turn, log: true);
                Renderer.Renderer.RenderToConsole(game.Board, clearFirst: false);

                var from = bestMove.Value.From;
                var to = bestMove.Value.To;

                return new int[] { from.row, from.col, to.row, to.col };
            }
            else
            {
                return "no moves left";
            }
        }
    }
}
