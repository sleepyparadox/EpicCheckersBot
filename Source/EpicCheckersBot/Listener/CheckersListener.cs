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

            Console.WriteLine("Recieved " + context.Request.RawUrl);

            // Parse board state

            RequestBody request;
            using (var inStream = new StreamReader(context.Request.InputStream))
            {
                var requestJson = inStream.ReadToEnd();
                request = JsonConvert.DeserializeObject<RequestBody>(requestJson);
            }

            // Todo create best move

            int fromRow = 0;
            int fromCol = 0;
            int toRow = 0;
            int toCol = 1;

            // Return best move

            var responseJavascript = string.Format("BootLoader.MoveUnit({0}, {1}, {2}, {3})", fromRow, fromCol, toRow, toCol);
            using (var outStream = new StreamWriter(context.Response.OutputStream))
            {
                outStream.Write(responseJavascript);
            }

            _httpListener.BeginGetContext(StartRequestProcess, null);
        }
    }
}
