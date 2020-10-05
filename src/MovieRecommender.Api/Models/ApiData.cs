using System.Collections.Generic;

namespace MovieRecommender.Api.Models
{
    public class ApiData<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }

    public class PagedApiData<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<T> Data { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public int PerPage { get; set; }
    }
}