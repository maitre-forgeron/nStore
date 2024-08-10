using Microsoft.AspNetCore.Mvc;

namespace NStore.Web.Models.QueryParameters
{
    [Serializable]
    public class PagedQueryParams 
    {
        private const int DEFAULT_PAGE_NUMBER = 1;
        private const int DEFAULT_PAGE_SIZE = 10;

        public PagedQueryParams() 
        {
        }

        [FromQuery(Name = "page")]
        public int Page { get; set; } = DEFAULT_PAGE_NUMBER;

        [FromQuery(Name = "limit")]
        public int Limit { get; set; } = DEFAULT_PAGE_SIZE;
    }
}
