namespace webapp.ViewModel
{
    public class PaginatedListViewModel<T>
    {
        public List<T> Items { get; set; }
        public uint CurrentPage { get; set; }
        public uint Limit { get; set; }
        public uint TotalPages { get; set; }
        public uint TotalItems { get; set; }
    }
    
}