namespace NStore.Web.Models.Shared
{
    public class PaginationViewModel
    {
        public int CurrentPageNumber { get; set; }

        public int ItemCount { get; set; }

        public int PageSize { get; set; }

        public int PageCount { get; set; }

        public string FirstPageUrl { get; set; }

        public string LastPageUrl { get; set; }

        public string? NextPageUrl { get; set; }

        public string? PreviousPageUrl { get; set; }

        public string CurrentPageUrl { get; set; }
    }
}
