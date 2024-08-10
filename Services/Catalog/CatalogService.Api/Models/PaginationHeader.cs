using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace CatalogService.Api.Models
{
    public sealed class PaginationHeader
    {
        public PaginationHeader(
            int currentPageNumber,
            int pageSize,
            int pageCount,
            int itemCount,
            string? nextPageUrl,
            string? previousPageUrl,
            string? firstPageUrl,
            string? lastPageUrl,
            string? currentPageUrl)
        {
            CurrentPageNumber = currentPageNumber;
            PageSize = pageSize;
            PageCount = pageCount;
            ItemCount = itemCount;
            NextPageUrl = nextPageUrl;
            PreviousPageUrl = previousPageUrl;
            FirstPageUrl = firstPageUrl;
            LastPageUrl = lastPageUrl;
            CurrentPageUrl = currentPageUrl;
        }

        public int CurrentPageNumber { get; }
        public int ItemCount { get; }
        public int PageSize { get; }
        public int PageCount { get; }
        public string? FirstPageUrl { get; }
        public string? LastPageUrl { get; }
        public string? NextPageUrl { get; }
        public string? PreviousPageUrl { get; }
        public string? CurrentPageUrl { get; }

        public string ToJsonString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public KeyValuePair<string, StringValues> ToKeyValuePair()
        {
            return new KeyValuePair<string, StringValues>("X-Pagination", this.ToJsonString());
        }
    }
}
