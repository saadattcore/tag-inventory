using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;


namespace Transcore.TagInventory.WindowsServices
{
    public class RestClient : IRestClient
    {
        #region Members
        public string _baseAddress;
        public int _timeOut;

        #endregion        

        #region Ctor

        public RestClient(string baseAddress, int timeOut)
        {
            _baseAddress = baseAddress;
            _timeOut = timeOut;
        }

        public RestClient(string baseAddress)
        {
            _baseAddress = baseAddress;
            _timeOut = 30;
        }

        #endregion

        #region Http methods

        public async Task<string> Get(string url)
        {
            string result = string.Empty;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseAddress);
                client.Timeout = new TimeSpan(0, _timeOut, 0);
                var response = client.GetAsync(url).Result;
                result = await response.Content.ReadAsStringAsync();

            }
            return result;
        }

        public async Task<WrapperResult> Post(string url, string json)
        {
            WrapperResult result = new WrapperResult();

            using (HttpClient client = new HttpClient())
            {
                client.Timeout = new TimeSpan(0, _timeOut, 0); // set time out as max value , due to api time out
                client.BaseAddress = new Uri(_baseAddress);
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Content = new StringContent(json);
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await client.SendAsync(request);

                result.Message = response.IsSuccessStatusCode ? response.ReasonPhrase : response.Content.ReadAsStringAsync().Result;
                result.Status = response.StatusCode;
                return result;
            }
        }

        #endregion

    }

    public class WrapperResult
    {

        public HttpStatusCode Status { get; set; }
        public string Message { get; set; }
    }
}
