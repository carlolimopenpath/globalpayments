using GlobalPayments.Api.Builders;
using GlobalPayments.Api.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GlobalPayments.Api.Gateways
{
    public class OpenPathGateway
    {
        public static OpenPathResponse SendRequest(string jsonContent, string url)
        {
            var result = new OpenPathResponse();
            // using (var client = new HttpClient() { Timeout = TimeSpan.FromMilliseconds(30000) })
            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Post, url))
            {
                using (var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json"))
                {
                    request.Content = stringContent;

                    using (var response = client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).Result)
                    {
                        response.EnsureSuccessStatusCode();
                        var responseJson = response.Content.ReadAsStringAsync().Result;
                        result = JsonConvert.DeserializeObject<OpenPathResponse>(responseJson);
                    }
                }
            }
            return result;
        }
    }
}
