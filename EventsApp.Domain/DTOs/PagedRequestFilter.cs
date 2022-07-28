namespace EventsApp.Domain.DTOs
{
    public class PagedRequestFilter
    {
        public string Searchtext { get; set; }
        public int EventId { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string SortBy { get; set; }
        public string SortDirection { get; set; }
    }
}
