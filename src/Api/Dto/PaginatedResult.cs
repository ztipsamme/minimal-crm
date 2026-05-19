using System;

namespace api.Dto
{

    public class PaginatedResult<T>
    {
        public List<T> Data { get; set; } = new();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
    }
}