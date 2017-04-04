using System.Linq;
using Microsoft.AspNetCore.Http;

namespace GST.Library.API.REST.Pagination
{
    /// <summary>
    /// Class to help holding attribute in HTTP's header for pagination
    /// </summary>
    /// Thanks to [Chsakell](https://chsakell.com/2016/06/23/rest-apis-using-asp-net-core-and-entity-framework-core/)
    public static class HttpResponseExtensions
    {
        /// <summary>
        /// Extension method to add pagination info to Response headers
        /// </summary>
        /// <param name="response"></param>
        /// <param name="currentPage"></param>
        /// <param name="itemsPerPage"></param>
        /// <param name="totalItems"></param>
        /// <param name="totalPages"></param>
        public static void AddPagination(this HttpResponse response, int currentPage, int itemsPerPage, int totalItems, int totalPages)
        {
            PaginationHeader paginationHeader = new PaginationHeader(currentPage, itemsPerPage, totalItems, totalPages);

            response.Headers.Add("Pagination", Newtonsoft.Json.JsonConvert.SerializeObject(paginationHeader));
            // CORS
            if (response.Headers.ContainsKey("access-control-expose-headers"))
            {
                response.Headers.Append("access-control-expose-headers", "Pagination");
            }
            else
            {
                response.Headers.Add("access-control-expose-headers", "Pagination");
            }
        }

        /// <summary>
        /// Extention method to add error to Response headers
        /// </summary>
        /// <param name="response"></param>
        /// <param name="message"></param>
        public static void AddApplicationError(this HttpResponse response, string message)
        {
            response.Headers.Add("Application-Error", message);
            // CORS
            if (response.Headers.ContainsKey("access-control-expose-headers"))
            {
                response.Headers.Append("access-control-expose-headers", "Application-Error");
            }
            else
            {
                response.Headers.Add("access-control-expose-headers", "Application-Error");
            }
        }
    }
}
