using CatalogService.Api.Models;
using Microsoft.AspNetCore.Mvc;
using NStore.Shared.Collections;

namespace CatalogService.Api.Extensions
{
    public static class HttpResponseExtensions
    {
        public static void AddPaginationHeader<T>(
            this HttpResponse response,
            IPagedCollection<T> items,
            string routeName,
            PagedQueryParams query,
            IUrlHelper url,
            bool includeOnlyQueryString = false)
        {
            if (response is null)
                throw new ArgumentNullException(nameof(response));

            if (items is null)
                throw new ArgumentNullException(nameof(items));

            if (string.IsNullOrEmpty(routeName))
                throw new ArgumentException($"'{routeName}' required", nameof(routeName));

            if (query is null)
                throw new ArgumentNullException(nameof(query));

            if (url is null)
                throw new ArgumentNullException(nameof(url));

            response.Headers.Add(items.ToPaginationHeader(routeName, query, url, includeOnlyQueryString).ToKeyValuePair());
        }

        public static void AddPaginationHeader<T>(
            this HttpResponse response,
            IPagedCollection<T> items,
            string routeName,
            PagedQueryParams query,
            LinkGenerator linkGenerator,
            HttpContext httpContext,
            bool includeOnlyQueryString = false)
        {
            if (response is null)
                throw new ArgumentNullException(nameof(response));

            if (items is null)
                throw new ArgumentNullException(nameof(items));

            if (string.IsNullOrEmpty(routeName))
                throw new ArgumentException($"'{routeName}' required", nameof(routeName));

            if (query is null)
                throw new ArgumentNullException(nameof(query));

            if (linkGenerator is null)
                throw new ArgumentNullException(nameof(linkGenerator));

            if (httpContext is null)
                throw new ArgumentNullException(nameof(httpContext));

            response.Headers.Add(items.ToPaginationHeader(routeName, query, linkGenerator, httpContext, includeOnlyQueryString).ToKeyValuePair());
        }

        public static void AddETagHeader(this HttpResponse response, string etag)
        {
            if (response is null)
                throw new ArgumentNullException(nameof(response));

            if (string.IsNullOrWhiteSpace(etag))
                throw new ArgumentException("message", nameof(etag));

            response.Headers.Add("ETag", etag);
        }

    }
}
