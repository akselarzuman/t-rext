using System.Threading.Tasks;
using t_rext.Models;

namespace t_rext.Contracts
{
    public interface IRestClient
    {
        Task<ApiResponse<T>> GetAsync<T>(string path, object request = null) where T : class, new();

        Task<ApiResponse<T>> PostAsync<T>(string path, object request = null) where T : class, new();

        Task<ApiResponse<T>> DeleteAsync<T>(string path, object request = null) where T : class, new();
        
        Task<ApiResponse<T>> PutAsync<T>(string path, object request = null) where T : class, new();
        
        Task<ApiResponse<T>> PatchAsync<T>(string path, object request = null) where T : class, new();
    }
}