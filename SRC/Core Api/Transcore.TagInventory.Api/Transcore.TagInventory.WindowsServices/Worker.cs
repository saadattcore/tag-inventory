using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace Transcore.TagInventory.WindowsServices
{
    public class Worker : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private static ManualResetEvent _manualResetEvent = new ManualResetEvent(false);
        private static List<TagReaderWrapper> _readers = new List<TagReaderWrapper>();


        public Worker(IConfiguration configuration)
        {
            //_logger = logger;
            _configuration = configuration;
            //_logger.Info("Worker constructore");
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {

            Log.Information("Start task async");

            try
            {
                short count = 1;

                while (true)
                {

                    string readerIP = _configuration.GetValue<string>("Reader" + count.ToString());

                    string userIP = _configuration.GetValue<string>("User" + count.ToString());

                    if (!string.IsNullOrEmpty(readerIP))
                    {

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

                                        Log.Debug($"Fetched userip is: ${userIP}");
                                    }
                                }

                            }
                        }

                        var apiEndPoint = _configuration.GetValue<string>("ApiEndpoint");

                        Log.Debug($"reader ip is: {readerIP} and userip is: {userIP} and apiendpoint is: {apiEndPoint}");

                        var reader = new TagReaderWrapper(readerIP, userIP, ref _manualResetEvent, apiEndPoint);

                        Task.Factory.StartNew(() => reader.StartTagReader());

                        _readers.Add(reader);

                        Log.Debug("Sucessfully started reader listner");

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
                Log.Error(ex.Message);
            }


            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            var signalStatus = _manualResetEvent.Set();

            if (signalStatus) Log.Debug("Wakeup signal send from OnStop");

            return base.StopAsync(cancellationToken);
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            //  _logger.Info("Execute Async");

            //while (!stoppingToken.IsCancellationRequested)
            //{

            //    // _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            //    await Task.Delay(1000, stoppingToken);
            //}           
        }
    }
}
