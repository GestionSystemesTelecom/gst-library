namespace GST.Library.API.REST.Pagination
{
    /// <summary>
    /// Class to help holding attribute in HTTP's header for pagination
    /// </summary>
    /// Thanks to [Chsakell](https://chsakell.com/2016/06/23/rest-apis-using-asp-net-core-and-entity-framework-core/)
    public class PaginationHeader
    {

        /// <summary>
        /// The current page to show
        /// </summary>
        public int CurrentPage { get; set; }
        /// <summary>
        /// The number of items to show per page
        /// </summary>
        public int ItemsPerPage { get; set; }
        /// <summary>
        /// The total number of items in the object
        /// </summary>
        public int TotalItems { get; set; }
        /// <summary>
        /// The total number of page to show all items of the object
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="itemsPerPage"></param>
        /// <param name="totalItems"></param>
        /// <param name="totalPages"></param>
        public PaginationHeader(int currentPage, int itemsPerPage, int totalItems, int totalPages)
        {
            this.CurrentPage = currentPage;
            this.ItemsPerPage = itemsPerPage;
            this.TotalItems = totalItems;
            this.TotalPages = totalPages;
        }
    }
}
