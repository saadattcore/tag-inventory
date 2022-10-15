using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Transcore.TagInventory.WindowsServices
{

    public class TagReaderWrapper
    {
        private static readonly ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Readers.E6.Reader _reader;
        private string _readerIP;
        private string _userIP;
        private ManualResetEvent _e;


        public TagReaderWrapper(string readerIP, string userIP, ref ManualResetEvent e)
        {
            _reader = new Readers.E6.Reader(IPAddress.Parse(readerIP));

            _e = e;
        }

        public void StartTagReader(string readerIP, string userIP)
        {
            _readerIP = readerIP;

            _userIP = userIP;

            //var _reader = new Readers.E6.Reader();

            try
            {

                _reader.ResponseTimeout.Add(TimeSpan.FromSeconds(60));

                _reader.HeartbeatInterval.Add(TimeSpan.FromSeconds(60));

                _reader.CommsState += HandleCommunication;

                _reader.TagRead += HandleTagRead;

                // connect to the reader

                _reader.Start();

                _logger.Info($"Sucesfully connected to reader = {readerIP} | for user = {userIP}");

                _e.WaitOne();

                if (_reader != null)
                {
                    _logger.Debug("Shutting down reader listner");

                    _reader.Dispose();
                }


            }
            catch (IOException e)
            {
                _logger.Error(e.Message, e);
            }
        }


        private void HandleTagRead(object sender, byte[] tagNumber)
        {
            var tagHex = BitConverter.ToString(tagNumber).Replace("-", "");

            var serialHex = new string(tagHex.ToList().GetRange(8, 6).ToArray());

            var serial = Convert.ToInt64(serialHex, 16);

            _logger.Info($"Tag number is {tagHex} | serial number = {serial} | reader = {_readerIP} | user = {_userIP}");

            Task.Factory.StartNew(() => PostMessage(tagHex, "read", serial));

        }

        private void HandleCommunication(object sender, bool communicating)
        {
            string message = string.Empty;

            if (communicating)
            {
                message = "Reader is up";

                _logger.Debug($"Message =  {message} | from reader = {_readerIP} | for user = {_userIP}");
            }
            else
            {
                message = "Reader is down";

                _logger.Warn($"Message =  {message} |  from reader = {_readerIP} |  for user = {_userIP}");
            }

            Task.Factory.StartNew(() => PostMessage(message, "commstate", 0));
        }

        private void PostMessage(string pMessage, string pType, long pSerialNumber)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string baseAddress = ConfigurationManager.AppSettings["ApiEndPoint"];

                    client.BaseAddress = new Uri(baseAddress);

                    client.Timeout = TimeSpan.FromHours(1);

                    JavaScriptSerializer js = new JavaScriptSerializer();

                    js.MaxJsonLength = int.MaxValue;

                    var payload = js.Serialize(new { message = pMessage, type = pType, userIP = _userIP, readerIP = _readerIP, serialNumber = pSerialNumber });

                    _logger.Debug($"Posting data={pMessage} | for user={_userIP} | from reader={_readerIP}");

                    HttpResponseMessage response = client.PostAsJsonAsync("TagReader", payload).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        _logger.Debug($"Sucesfully posted the data");
                    }
                    else
                    {
                        _logger.Debug($"Request for posting data failed");

                        var reasonPhrase = response.Content.ReadAsStringAsync().Result;

                        _logger.Debug($"Status code = {response.StatusCode} | reason pharase = {response.ReasonPhrase} | error = {reasonPhrase}");
                    }

                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);

                if (ex.InnerException != null)
                {
                    _logger.Error(ex.InnerException.Message, ex.InnerException);
                }
            }
        }
    }
}

