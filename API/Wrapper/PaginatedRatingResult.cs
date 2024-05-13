namespace API.Wrapper
{
    public class PaginatedRatingResult<T> : Result
    {
        public List<T> Data
        {
            get; set;
        }


        public PaginatedRatingResult(bool succeeded, List<T> data = default, string message = null, int count = 0, int page = 1, int pageSize = 10, double totalAverage = 0.0)
        {
            Data = data;
            CurrentPage = page;
            Succeeded = succeeded;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalCount = count;
            TotalAverage = totalAverage;
            Message = message;
        }

        public double TotalAverage
        {
            get; set;
        }

        public int CurrentPage
        {
            get; set;
        }

        public int TotalPages
        {
            get; set;
        }

        public int TotalCount
        {
            get; set;
        }

        public int UnreadCount
        {
            get; set;
        }
        public int PageSize
        {
            get; set;
        }

        public string Message
        {
            get; set;
        }

    }
}
