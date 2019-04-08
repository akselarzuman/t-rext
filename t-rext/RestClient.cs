using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using t_rext.Contracts;
using t_rext.Core;
using t_rext.Models;

namespace t_rext
{
    public class RestClient : IRestClient
    {
        private readonly HttpClient _client;
        private JsonSerializerSettings _jsonSerializerSettings;

        public RestClient(HttpClient client)
        {
            _client = client;
        }

        public void AddSerializerSettings(JsonSerializerSettings jsonSerializerSettings)
        {
            _jsonSerializerSettings = jsonSerializerSettings;
        }

        public async Task<ApiResponse<T>> GetAsync<T>(
            string path,
            IList<KeyValuePair<string, string>> queryParams = null,
            IDictionary<string, string> headerParams = null,
            IList<KeyValuePair<string, string>> formData = null,
            object bodyParams = null,
            OAuth2HeaderModel oAuth2HeaderModel = null) where T : class, new()
        {
            return await SendAsync<T>(path, HttpMethod.Get, queryParams, headerParams, formData, bodyParams, oAuth2HeaderModel);
        }

        public async Task<ApiResponse<T>> PostAsync<T>(
            string path,
            IList<KeyValuePair<string, string>> queryParams = null,
            IDictionary<string, string> headerParams = null,
            IList<KeyValuePair<string, string>> formData = null,
            object bodyParams = null,
            OAuth2HeaderModel oAuth2HeaderModel = null) where T : class, new()
        {
            return await SendAsync<T>(path, HttpMethod.Post, queryParams, headerParams, formData, bodyParams, oAuth2HeaderModel);
        }

        public async Task<ApiResponse<T>> DeleteAsync<T>(
            string path,
            IList<KeyValuePair<string, string>> queryParams = null,
            IDictionary<string, string> headerParams = null,
            IList<KeyValuePair<string, string>> formData = null,
            object bodyParams = null,
            OAuth2HeaderModel oAuth2HeaderModel = null) where T : class, new()
        {
            return await SendAsync<T>(path, HttpMethod.Delete, queryParams, headerParams, formData, bodyParams, oAuth2HeaderModel);
        }

        public async Task<ApiResponse<T>> PutAsync<T>(
            string path,
            IList<KeyValuePair<string, string>> queryParams = null,
            IDictionary<string, string> headerParams = null,
            IList<KeyValuePair<string, string>> formData = null,
            object bodyParams = null,
            OAuth2HeaderModel oAuth2HeaderModel = null) where T : class, new()
        {
            return await SendAsync<T>(path, HttpMethod.Put, queryParams, headerParams, formData, bodyParams, oAuth2HeaderModel);
        }

        public async Task<ApiResponse<T>> PatchAsync<T>(
            string path,
            IList<KeyValuePair<string, string>> queryParams = null,
            IDictionary<string, string> headerParams = null,
            IList<KeyValuePair<string, string>> formData = null,
            object bodyParams = null,
            OAuth2HeaderModel oAuth2HeaderModel = null) where T : class, new()
        {
            return await SendAsync<T>(path, new HttpMethod("PATCH"), queryParams, headerParams, formData, bodyParams, oAuth2HeaderModel);
        }

        private async Task<ApiResponse<T>> SendAsync<T>(
            string path,
            HttpMethod httpMethod,
            IList<KeyValuePair<string, string>> queryParams = null,
            IDictionary<string, string> headerParams = null,
            IList<KeyValuePair<string, string>> formData = null,
            object bodyParams = null,
            OAuth2HeaderModel oAuth2HeaderModel = null) where T : class, new()
        {
            Ensure.ArgumentNotNullOrEmptyString(path, nameof(path));

            path.AddQueryParameters(queryParams);

            using (var requestMessage = new HttpRequestMessage
            {
                Method = httpMethod,
                RequestUri = new Uri(path, UriKind.RelativeOrAbsolute)
            })
            {
                requestMessage.AddHeaders(headerParams);
                requestMessage.AddBody(bodyParams, _jsonSerializerSettings);
                requestMessage.AddFormUrlEncodedContent(formData);
                requestMessage.AddBasicOAuth2Header(oAuth2HeaderModel);

                using (HttpResponseMessage httpResponseMessage = await _client.SendAsync(requestMessage).ConfigureAwait(false))
                {
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
                        && apiResponse.Headers["Content-Type"].Contains("application/json"))
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
    }
}