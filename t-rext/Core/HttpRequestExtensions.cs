using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using Newtonsoft.Json;

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
    }
}