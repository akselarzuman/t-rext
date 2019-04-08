using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using t_rext.Models;

namespace t_rext.Core
{
    public static class HttpRequestExtensions
    {
        public static void AddQueryParameters(this string path, IList<KeyValuePair<string, string>> queryParams)
        {
            Ensure.ArgumentNotNullOrEmptyString(path, nameof(path));

            if (queryParams != null && queryParams.Any())
            {
                var query = HttpUtility.ParseQueryString(string.Empty);

                foreach (var queryParam in queryParams)
                {
                    query[queryParam.Key] = queryParam.Value;
                }

                path = $"{path}?{query}";
            }
        }

        public static void AddHeaders(this HttpRequestMessage httpRequestMessage, IDictionary<string, string> headerParams)
        {
            if (headerParams != null && headerParams.Any())
            {
                foreach (var header in headerParams)
                {
                    httpRequestMessage.Headers.Add(header.Key, header.Value);
                }
            }
        }

        public static void AddBody(this HttpRequestMessage httpRequestMessage, object bodyParams, JsonSerializerSettings jsonSerializerSettings)
        {
            Ensure.ArgumentNotNull(jsonSerializerSettings, nameof(jsonSerializerSettings));

            if (bodyParams != null)
            {
                string request = JsonConvert.SerializeObject(bodyParams, jsonSerializerSettings);
                httpRequestMessage.Content = new StringContent(request);
            }
        }

        public static void AddFormUrlEncodedContent(this HttpRequestMessage requestMessage, IList<KeyValuePair<string, string>> formData)
        {
            if (formData != null && formData.Any())
            {
                requestMessage.Content = new FormUrlEncodedContent(formData);
            }
        }

        public static void AddBasicOAuth2Header(this HttpRequestMessage httpRequestMessage, OAuth2HeaderModel oAuth2HeaderModel)
        {
            if (oAuth2HeaderModel != null)
            {
                string credential = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{oAuth2HeaderModel.Key}:{oAuth2HeaderModel.Secret}"));

                httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", credential);
            }
        }
    }
}