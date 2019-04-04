using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using t_rext.Contracts;
using t_rext.Models;

namespace t_rext
{
    public class RestClient : IRestClient
    {
        private readonly HttpClient _client;

        public RestClient(Func<string, HttpClient> clientFactory)
        {
            _client = clientFactory("IterableClient");
        }

        public RestClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<ApiResponse<T>> GetAsync<T>(string path, object request = null) where T : class, new()
        {
            return await SendAsync<T>(path, HttpMethod.Get, request);
        }

        public async Task<ApiResponse<T>> PostAsync<T>(string path, object request = null) where T : class, new()
        {
            return await SendAsync<T>(path, HttpMethod.Post, request);
        }

        public async Task<ApiResponse<T>> DeleteAsync<T>(string path, object request = null) where T : class, new()
        {
            return await SendAsync<T>(path, HttpMethod.Delete, request);
        }

        public async Task<ApiResponse<T>> PutAsync<T>(string path, object request = null) where T : class, new()
        {
            return await SendAsync<T>(path, HttpMethod.Put, request);
        }

        public async Task<ApiResponse<T>> PatchAsync<T>(string path, object request = null) where T : class, new()
        {
            return await SendAsync<T>(path, new HttpMethod("PATCH"), request);
        }

        private async Task<ApiResponse<T>> SendAsync<T>(string path, HttpMethod httpMethod, object request = null) where T : class, new()
        {
            Ensure.ArgumentNotNullOrEmptyString(path, nameof(path));

            HttpRequestMessage requestMessage = new HttpRequestMessage
            {
                Method = httpMethod,
                RequestUri = new Uri(path, UriKind.RelativeOrAbsolute)
            };

            if (request != null)
            {
                requestMessage.Content = request.Serialize();
            }

            HttpResponseMessage httpResponseMessage = await _client.SendAsync(requestMessage).ConfigureAwait(false);
            string content = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

            var apiResponse = new ApiResponse<T>
            {
                HttpStatusCode = httpResponseMessage.StatusCode,
                Headers = httpResponseMessage.Content.Headers.ToDictionary(pair => pair.Key, pair => pair.Value.First()),
                UrlPath = path
            };

            if (apiResponse.HttpStatusCode != HttpStatusCode.BadRequest
                && apiResponse.HttpStatusCode != HttpStatusCode.Unauthorized
                && !string.IsNullOrEmpty(content)
                && apiResponse.Headers["Content-Type"].Equals("application/json"))
            {
                apiResponse.Model = JsonConvert.DeserializeObject<T>(content);
            }
            else
            {
                apiResponse.Error = true;
                apiResponse.Content = content;
            }

            return apiResponse;
        }
    }
}