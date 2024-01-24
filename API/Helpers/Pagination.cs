namespace API.Helpers
{
    public class Pagination<T> where T: class
    {
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public IReadOnlyList<T> Data { get; set; }

        public Pagination(int pageIndex, int totalPages, int pageSize, int totalItems, IReadOnlyList<T> data)
        {
            PageIndex = pageIndex;
            TotalPages = totalPages;
            PageSize = pageSize;
            TotalItems = totalItems;
            Data = data;
        }

        // Optional: Method to calculate total pages (can also be done in the constructor)
        public static int CalculateTotalPages(int totalItems, int pageSize)
        {
            return (int)Math.Ceiling(totalItems / (double)pageSize);
        }
    }
}