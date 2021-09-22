using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace Infra.Authentication
{
    public class BasicAuthClient : IBasicAuthClient
    {
        private const string _username = "mobyyou4";
        private const string _token = "<Token>";
        private readonly byte[] _basicAuthentication;
        private HttpClient _client;
        public BasicAuthClient()
        {
            _basicAuthentication = Encoding.ASCII.GetBytes($"{_username}:{_token}");
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(_basicAuthentication));
            _client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("XXX", "1.0"));
        }

        public async Task<(HttpStatusCode, string)> GetAsync(string APIAddress)
        {
            try
            {
                var result = await _client.GetAsync(APIAddress);
                string content = string.Empty;
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    content = await result.Content.ReadAsStringAsync();
                }
                else
                {
                    throw new HttpRequestException($"invalid http code:{result.StatusCode}");
                }
                Console.WriteLine(content);
                return (HttpStatusCode.OK, content);
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
            return (HttpStatusCode.InternalServerError, string.Empty);
        }

        public HttpStatusCode PatchAsync(string APIAddress, string body)
        {
            try
            {
                var client = new RestClient(APIAddress);
                client.Timeout = -1;
                var request = new RestRequest(Method.PATCH);
                request.AddHeader("Authorization", "Basic " + Convert.ToBase64String(_basicAuthentication));
                request.AddParameter("text/plain", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                return response.StatusCode;

                //TODO: replace this to HTTPClient which currently with this code PATCH call ends with bad request
                //var stringContent = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
                //var result = await _client.PatchAsync(APIAddress, stringContent);
                //string content = string.Empty;
                //if (result.StatusCode == HttpStatusCode.OK)
                //{
                //    content = await result.Content.ReadAsStringAsync();
                //}
                //else
                //{
                //    throw new HttpRequestException($"invalid http code:{result.StatusCode}");
                //}
                //Console.WriteLine(content);
                //return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return HttpStatusCode.InternalServerError;
        }
    }
}
