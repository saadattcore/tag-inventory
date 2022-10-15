using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Transcore.TagInventory.WindowsServices
{
    public partial class Service1 : ServiceBase
    {
        private static readonly ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static List<TagReaderWrapper> _readers = new List<TagReaderWrapper>();
        private static ManualResetEvent _manualResetEvent = new ManualResetEvent(false);
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {

            RunTaksAsync();
        }

        private void RunTaksAsync()
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
                        var host = Dns.GetHostEntry(Dns.GetHostName());

                        var ipv4 = host.AddressList.FirstOrDefault(i => i.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);

                        userIP = ipv4.ToString();
                    }

                    _logger.Debug($"User ip address is = {userIP}");

                    if (readerIP != null && userIP != null)
                    {
                        var r = new TagReaderWrapper(readerIP, userIP, ref _manualResetEvent);

                        //_readers.Add(r);

                        Task.Factory.StartNew(() => r.StartTagReader(readerIP, userIP));

                        // r.StartTagReader(readerIP, userIP);



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

        protected override void OnStop()
        {
            var signalStatus = _manualResetEvent.Set();

            if (signalStatus) _logger.Debug("Wakeup signal send from OnStop");
        }
    }
}
