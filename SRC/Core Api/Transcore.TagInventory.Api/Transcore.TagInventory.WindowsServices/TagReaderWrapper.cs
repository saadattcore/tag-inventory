using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net.Http.Json;
using Serilog;

namespace Transcore.TagInventory.WindowsServices
{
    public class TagReaderWrapper
    {
        //private static readonly ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Readers.E6.Reader _reader;
        private readonly string _readerIP;
        private readonly string _userIP;
        private readonly ManualResetEvent _e;
        private readonly string _apiEndPoint;


        public TagReaderWrapper(string readerIP, string userIP, ref ManualResetEvent e, string apiEndPoint)
        {
            _readerIP = readerIP;
            _userIP = userIP;

            _reader = new Readers.E6.Reader(IPAddress.Parse(readerIP));

            _e = e;

            _apiEndPoint = apiEndPoint;

        }

        public void StartTagReader()
        {
            try
            {

                _reader.ResponseTimeout.Add(TimeSpan.FromSeconds(60));

                _reader.HeartbeatInterval.Add(TimeSpan.FromSeconds(60));

                _reader.CommsState += HandleCommunication;

                _reader.TagRead += HandleTagRead;

                // connect to the reader                

                _reader.Start();

                Log.Information($"Sucesfully connected to reader = {_readerIP} | for user = {_userIP}");

                _e.WaitOne();

                if (_reader != null)
                {
                    Log.Debug("Shutting down reader listner");

                    _reader.Dispose();
                }


            }
            catch (IOException e)
            {
                Log.Error(e.Message, e);
            }
        }


        private void HandleTagRead(object sender, byte[] tagNumber)
        {
            var tagHex = BitConverter.ToString(tagNumber).Replace("-", "");

            var serialHex = new string(tagHex.ToList().GetRange(8, 6).ToArray());

            var serial = Convert.ToInt64(serialHex, 16);

            Log.Information($"Tag number is {tagHex} | serial number = {serial} | reader = {_readerIP} | user = {_userIP}");

            Task.Factory.StartNew(() => PostMessage(tagHex, "read", serial));

        }

        private void HandleCommunication(object sender, bool communicating)
        {
            string message = string.Empty;

            if (communicating)
            {
                message = "Reader is up";

                Log.Debug($"Message =  {message} | from reader = {_readerIP} | for user = {_userIP}");
            }
            else
            {
                message = "Reader is down";

                Log.Warning($"Message =  {message} |  from reader = {_readerIP} |  for user = {_userIP}");
            }

            Task.Factory.StartNew(() => PostMessage(message, "commstate", 0));
        }

        private void PostMessage(string pMessage, string pType, long pSerialNumber)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_apiEndPoint);

                    client.Timeout = TimeSpan.FromHours(1);

                    var payload = JsonSerializer.Serialize(new { message = pMessage, type = pType, userIP = _userIP, readerIP = _readerIP, serialNumber = pSerialNumber });

                    Log.Debug($"Posting data={pMessage} | for user={_userIP} | from reader={_readerIP} | at api= {_apiEndPoint}");

                    HttpResponseMessage response = client.PostAsJsonAsync("TagReader", payload).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        Log.Debug($"Sucesfully posted the data");
                    }
                    else
                    {
                        Log.Debug($"Request for posting data failed");

                        var reasonPhrase = response.Content.ReadAsStringAsync().Result;

                        Log.Debug($"Status code = {response.StatusCode} | reason pharase = {response.ReasonPhrase} | error = {reasonPhrase}");
                    }

                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);

                if (ex.InnerException != null)
                {
                    Log.Error(ex.InnerException.Message, ex.InnerException);
                }
            }
        }
    }
}

