using System.Collections.Generic;
using System.Net;

namespace t_rext.Models
{
    public class ApiResponse<TModel> : ApiResponse where TModel : class, new()
    {
        public TModel Model { get; set; }
    }

    public class ApiResponse
    {
        public HttpStatusCode HttpStatusCode { get; set; }

        public IDictionary<string, string> Headers { get; set; }

        public string UrlPath { get; set; }

        public string Content { get; set; }

        public bool Error { get; set; }
    }
}