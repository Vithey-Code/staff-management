namespace API.Wrapper
{
    public class PaginatedResult<T> : Result
    {
        public PaginatedResult(List<T> data)
        {
            Data = data;
        }

        public List<T> Data
        {
            get; set;
        }

        public PaginatedResult(bool succeeded, int unreadCount = 0, List<T> data = default, string messages = null, int count = 0, int page = 1, int pageSize = 10)
        {
            Data = data;
            CurrentPage = page;
            Succeeded = succeeded;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalCount = count;
            UnreadCount = unreadCount;
            Messages = messages;
        }

        public PaginatedResult(bool succeeded, List<T> data = default, List<string> messages = null, int count = 0, int todayCount = 0, int page = 1, int pageSize = 10)
        {
            Data = data;
            CurrentPage = page;
            Succeeded = succeeded;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalCount = count;
            TodayCount = todayCount;
        }

        public static PaginatedRatingResult<T> RatingResult(bool succeeded, List<T> data = default, string messages = null, int count = 0, int page = 1, int pageSize = 10, double totalAverage = 0.0)
        {
            return new PaginatedRatingResult<T>(succeeded, data, messages, count, page, pageSize, totalAverage);
        }

        public static PaginatedResult<T> Failure(List<string> messages)
        {
            return new PaginatedResult<T>(false, default, messages, 0, 1, 10);
        }

        public static PaginatedResult<T> Success(int unreadCount, List<T> data, int count, int page, int pageSize, string messages = null)
        {
            return new PaginatedResult<T>(true, unreadCount, data, messages, count, page, pageSize);
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

        public int TodayCount
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

        public bool HasPreviousPage => CurrentPage > 1;

        public bool HasNextPage => CurrentPage < TotalPages;
    }

}