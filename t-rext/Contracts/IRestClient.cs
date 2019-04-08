using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using t_rext.Models;

namespace t_rext.Contracts
{
    public interface IRestClient
    {
        void AddSerializerSettings(JsonSerializerSettings jsonSerializerSettings);
        
        Task<ApiResponse<T>> GetAsync<T>(
            string path,
            IList<KeyValuePair<string, string>> queryParams = null,
            IDictionary<string, string> headerParams = null,
            IList<KeyValuePair<string, string>> formData = null,
            object bodyParams = null) where T : class, new();

        Task<ApiResponse<T>> PostAsync<T>(
            string path,
            IList<KeyValuePair<string, string>> queryParams = null,
            IDictionary<string, string> headerParams = null,
            IList<KeyValuePair<string, string>> formData = null,
            object bodyParams = null) where T : class, new();

        Task<ApiResponse<T>> DeleteAsync<T>(
            string path,
            IList<KeyValuePair<string, string>> queryParams = null,
            IDictionary<string, string> headerParams = null,
            IList<KeyValuePair<string, string>> formData = null,
            object bodyParams = null) where T : class, new();

        Task<ApiResponse<T>> PutAsync<T>(
            string path,
            IList<KeyValuePair<string, string>> queryParams = null,
            IDictionary<string, string> headerParams = null,
            IList<KeyValuePair<string, string>> formData = null,
            object bodyParams = null) where T : class, new();

        Task<ApiResponse<T>> PatchAsync<T>(
            string path,
            IList<KeyValuePair<string, string>> queryParams = null,
            IDictionary<string, string> headerParams = null,
            IList<KeyValuePair<string, string>> formData = null,
            object bodyParams = null) where T : class, new();
    }
}