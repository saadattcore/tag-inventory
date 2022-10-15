using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Transcore.TagInventory.WindowsServices
{
    static class Program
    {

        private static List<TagReaderWrapper> _readers = new List<TagReaderWrapper>();
        private static readonly ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static ManualResetEvent manualResetEvent = new ManualResetEvent(false);

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {


            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new Service1()
            };
            ServiceBase.Run(ServicesToRun);

        }

        private static void RunTaksAsync()
        {
            try
            {
                short count = 1;

                while (true)
                {
                    string readerIP = ConfigurationManager.AppSettings["Reader" + count.ToString()];

                    string userIP = ConfigurationManager.AppSettings["User" + count.ToString()];

                    if (string.IsNullOrEmpty(userIP))
                    {

                        foreach (var ni in NetworkInterface.GetAllNetworkInterfaces())
                        {
                            var addr = ni.GetIPProperties().GatewayAddresses.FirstOrDefault();

                            if (addr != null)
                            {
                                if (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                                {
                                    UnicastIPAddressInformation ip = ni.GetIPProperties().UnicastAddresses.FirstOrDefault(i => i.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
                                    userIP = ip.Address.ToString();
                                }
                            }

                        }
                    }

                    if (readerIP != null)
                    {
                        var r = new TagReaderWrapper(readerIP, userIP, ref manualResetEvent);

                        _readers.Add(r);

                        Task.Factory.StartNew(() => r.StartTagReader(readerIP, userIP));

                        _logger.Debug("I hope thread will not reach to this point");

                    }
                    else
                    {
                        break;
                    }
                    count++;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }

        }

        private static void PostMessage(string msg, string ty)
        {
            using (HttpClient client = new HttpClient())
            {
                string baseAddress = ConfigurationManager.AppSettings["ApiEndPoint"];

                client.BaseAddress = new Uri(baseAddress);

                client.Timeout = TimeSpan.FromHours(1);


                try
                {
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "TagReader");

                    JavaScriptSerializer js = new JavaScriptSerializer();

                    js.MaxJsonLength = int.MaxValue;

                    var payload = js.Serialize(new { message = msg, type = ty, userIP = "192.168.1.138", readerIP = "0.0.0.0" });

                    _logger.Debug($"Posting data={msg} for user=192.168.1.138 from reader=0.0.0.0");

                    HttpResponseMessage response = client.PostAsJsonAsync("TagReader", payload).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        _logger.Debug($"Sucesfully posted the data");

                    }
                    else
                    {
                        _logger.Debug($"Request for posting data failed");

                        var reasonPhrase = response.Content.ReadAsStringAsync().Result;

                        _logger.Debug($"status code = {response.StatusCode} , {response.ReasonPhrase} error = {reasonPhrase}");
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
}
