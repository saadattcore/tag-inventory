using log4net;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Text;
using System.Threading;
using Serilog;
using Microsoft.AspNetCore.Http;

namespace Transcore.TagInventory.Web.Controllers
{
    [Route("api/tagreader")]
    [ApiController]
    public class TagReaderController : ControllerBase
    {

        private static ConcurrentDictionary<string, StreamWriter> _connections = new ConcurrentDictionary<string, StreamWriter>();

        private byte[] nextLine = ASCIIEncoding.ASCII.GetBytes("\n");

        public TagReaderController(ILog logger)
        {

        }

        [HttpGet]
        public async Task Get()
        {
            Response.Headers["Cache-Control"] = "no-cache";
            Response.Headers["X-Accel-Buffering"] = "no";
            Response.Headers["Connection"] = "keep-alive";
            Response.ContentType = "text/event-stream";

            Log.Debug("Get request for SSE");

            string ipAdd = Request.HttpContext.Connection.RemoteIpAddress.ToString();

            Log.Debug($"Connected web user ip= {ipAdd}");

            var sw = new StreamWriter(HttpContext.Response.Body);

            if (!_connections.TryAdd(ipAdd, sw))
            {
                StreamWriter exitingConnection = null;

                _connections.Remove(ipAdd, out exitingConnection);

                _connections.TryAdd(ipAdd, sw);
            }

            await sw.WriteAsync("event: connected\ndata:\n\n");
            await sw.FlushAsync();

            try
            {
                await Task.Delay(Timeout.Infinite, HttpContext.RequestAborted);
            }
            catch (TaskCanceledException)
            {
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] string msg)
        {
            if (string.IsNullOrEmpty(msg))
                return BadRequest("Please send proper message");

            if (_connections.Count == 0)
            {
                string error = "No active connection  found";

                Log.Warning(error);

                return NotFound(error);

            }

            string responseToWrite = string.Empty;

            var payloadKV = ParseValidateRequest(msg, out responseToWrite);

            string userIP = payloadKV["userIP"];

            try
            {

                StreamWriter userConnection = _connections[userIP];

                async Task Send(StreamWriter member)
                {
                    await member.WriteAsync("data: " + responseToWrite + "\n\n");
                    await member.FlushAsync();
                }

                List<StreamWriter> swList = new List<StreamWriter>() { userConnection };

                await Task.WhenAll(swList.Select(Send));
            }
            catch (System.Collections.Generic.KeyNotFoundException e)
            {
                string error = $"Cannot find connection for ip = {userIP} existing";

                Log.Error(e.Message, e);

                return NotFound(error);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);

                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok();
        }

        private Dictionary<string, string> ParseValidateRequest(string pMessage, out string responseToWrite)
        {
            responseToWrite = "";
            var kvPayload = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(pMessage);

            //JavaScriptSerializer js = new JavaScriptSerializer();

            // object o = js.DeserializeObject(pMessage);

            // var kvPayload = (Dictionary<string, object>)o;

            if (!kvPayload.ContainsKey("message") || kvPayload["message"] == null)
                throw new ArgumentException("Message missing or invalid value");

            if (!kvPayload.ContainsKey("type") || kvPayload["type"] == null)
                throw new ArgumentException("Message type missing or invalid value");

            if (!kvPayload.ContainsKey("readerIP") || kvPayload["readerIP"] == null)
                throw new ArgumentException("ReaderIP missing or invalid value");

            if (!kvPayload.ContainsKey("userIP") || kvPayload["userIP"] == null)
                throw new ArgumentException("UserIP missing or invalid value");

            if (!kvPayload.ContainsKey("serialNumber") || kvPayload["serialNumber"] == null)
                throw new ArgumentException("Serialnumber missing or invalid value");


            var resultDictionary = new Dictionary<string, string>();

            string message = kvPayload["message"].ToString();

            string type = kvPayload["type"].ToString();

            string readerIP = kvPayload["readerIP"].ToString();

            string userIP = kvPayload["userIP"].ToString();

            long serialNumber = 0;

            if (!long.TryParse(kvPayload["serialNumber"].ToString(), out serialNumber))
            {
                throw new ArgumentException("Invalid serial number value");
            }

            resultDictionary.Add("message", message);

            resultDictionary.Add("type", type);

            resultDictionary.Add("readerIP", readerIP);

            resultDictionary.Add("userIP", userIP);

            resultDictionary.Add("serialnumber", serialNumber.ToString());

            responseToWrite = Newtonsoft.Json.JsonConvert.SerializeObject(new { message = message, serialNumber = serialNumber, messageType = type });

            return resultDictionary;



        }
    }


}
