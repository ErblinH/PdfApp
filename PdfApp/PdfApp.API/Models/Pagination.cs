namespace PdfApp.API.Models
{
    public class Pagination
    {
        public int TotalCount { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public bool HasNextPage => PageIndex + 1 < TotalPages;
        public bool HasPreviousPage => PageIndex > 0;
    }
}
