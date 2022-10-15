using log4net;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace Transcore.TagInventory.Web.Controllers
{
    public class TagReaderController : ApiController
    {

        private static ConcurrentDictionary<string, StreamWriter> _connections = new ConcurrentDictionary<string, StreamWriter>();
        private readonly ILog _logger;

        public TagReaderController(ILog logger)
        {
            _logger = logger;
        }


        public HttpResponseMessage Get()
        {
            _logger.Debug("Get request for SSE");

            var response = Request.CreateResponse(HttpStatusCode.OK);            

            response.Content = new PushStreamContent((Action<Stream, HttpContent, TransportContext>)StreamAvailable, "text/event-stream");

            return response;
        }

        private void StreamAvailable(Stream stream, HttpContent content, TransportContext context)
        {
            string ipAdd = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (string.IsNullOrEmpty(ipAdd))
            {
                ipAdd = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            _logger.Debug($"Client ip address = {ipAdd}");

            StreamWriter toRemove = null;

            var currentConnection = _connections.SingleOrDefault(con => con.Key == ipAdd);

            if (currentConnection.Key != null)
            {
                _connections.TryRemove(ipAdd, out toRemove);
            }

            if(_connections.TryAdd(ipAdd, new StreamWriter(stream)))
            {
                _logger.Debug($"Added connection for ip = {ipAdd}");
            }
            
        }


        [HttpPost]
        public IHttpActionResult Post([FromBody] string msg)
        {
            if (string.IsNullOrEmpty(msg))
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Please send proper message"));

            if (_connections.Count == 0)
            {
                string error = "No active connection  found";

                _logger.Warn(error);

                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, error));

            }

            string responseToWrite = string.Empty;

            var payloadKV = ParseValidateRequest(msg, out responseToWrite);

            string userIP = payloadKV["userIP"];

            try
            {

                var userConnection = _connections[userIP];

                userConnection.AutoFlush = true;

                userConnection.WriteLine("data:" + responseToWrite);

                userConnection.WriteLine();

                userConnection.Flush();

                userConnection.WriteLine();

                //userConnection.Flush();
            }
            catch (System.Collections.Generic.KeyNotFoundException e)
            {
                string error = $"Cannot find connection for ip = {userIP} existing";

                _logger.Error(e.Message, e);

                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, error));
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);

                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }


            return Ok("Sucess");
        }

        private Dictionary<string, string> ParseValidateRequest(string pMessage, out string responseToWrite)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            object o = js.DeserializeObject(pMessage);

            var kvPayload = (Dictionary<string, object>)o;

            if (!kvPayload.ContainsKey("message") || kvPayload["message"] == null)
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Message missing or invalid value"));

            if (!kvPayload.ContainsKey("type") || kvPayload["type"] == null)
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Message type missing or invalid value"));

            if (!kvPayload.ContainsKey("readerIP") || kvPayload["readerIP"] == null)
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "ReaderIP missing or invalid value"));

            if (!kvPayload.ContainsKey("userIP") || kvPayload["userIP"] == null)
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "UserIP missing or invalid value"));

            if (!kvPayload.ContainsKey("serialNumber") || kvPayload["serialNumber"] == null)
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Serialnumber missing or invalid value"));


            var resultDictionary = new Dictionary<string, string>();

            string message = kvPayload["message"].ToString();

            string type = kvPayload["type"].ToString();

            string readerIP = kvPayload["readerIP"].ToString();

            string userIP = kvPayload["userIP"].ToString();

            long serialNumber = 0;

            if (!long.TryParse(kvPayload["serialNumber"].ToString(), out serialNumber))
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid serial number value"));
            }

            resultDictionary.Add("message", message);

            resultDictionary.Add("type", type);

            resultDictionary.Add("readerIP", readerIP);

            resultDictionary.Add("userIP", userIP);

            resultDictionary.Add("serialnumber", serialNumber.ToString());

            responseToWrite = js.Serialize(new { message = message, serialNumber = serialNumber, messageType = type });

            return resultDictionary;

        }
    }
}
