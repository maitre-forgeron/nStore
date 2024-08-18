using CatalogService.Api.Models;
using Microsoft.AspNetCore.Mvc;
using NStore.Shared.Collections;

namespace CatalogService.Api.Extensions
{
    public static class PagedListExtensions
    {
        public static PaginationHeader ToPaginationHeader<T>(
            this IPagedCollection<T> pagedCollection,
            string routeName,
            PagedQueryParams queryParams,
            IUrlHelper urlHelper,
            bool includeOnlyQueryString)
        {
            if (pagedCollection is null)
                throw new ArgumentNullException(nameof(pagedCollection));

            if (string.IsNullOrEmpty(routeName))
                throw new ArgumentException("message", nameof(routeName));

            if (queryParams is null)
                throw new ArgumentNullException(nameof(queryParams));

            if (urlHelper is null)
                throw new ArgumentNullException(nameof(urlHelper));

            return new PaginationHeader(
                pagedCollection.CurrentPageNumber,
                pagedCollection.PageSize,
                pagedCollection.PageCount,
                pagedCollection.ItemCount,
                GetModifiedUrl(urlHelper.LinkNextPage(routeName, pagedCollection.PageSize, pagedCollection.NextPageNumber, queryParams)?.ToString(), includeOnlyQueryString),
                GetModifiedUrl(urlHelper.LinkPreviousPage(routeName, pagedCollection.PageSize, pagedCollection.PreviousPageNumber, queryParams)?.ToString(), includeOnlyQueryString),
                GetModifiedUrl(urlHelper.LinkFirstPage(routeName, pagedCollection.PageSize, queryParams)?.ToString(), includeOnlyQueryString),
                GetModifiedUrl(urlHelper.LinkLastPage(routeName, pagedCollection.PageSize, pagedCollection.LastPageNumber, queryParams)?.ToString(), includeOnlyQueryString),
                GetModifiedUrl(urlHelper.LinkCurrentPage(routeName, pagedCollection.PageSize, pagedCollection.CurrentPageNumber, queryParams)?.ToString(), includeOnlyQueryString));
        }

        public static PaginationHeader ToPaginationHeader<T>(
            this IPagedCollection<T> pagedCollection,
            string routeName,
            PagedQueryParams queryParams,
            LinkGenerator linkGenerator,
            HttpContext httpContext,
            bool includeOnlyQueryString)
        {
            if (pagedCollection is null)
                throw new ArgumentNullException(nameof(pagedCollection));

            if (string.IsNullOrEmpty(routeName))
                throw new ArgumentException("Route name cannot be null or empty.", nameof(routeName));

            if (queryParams is null)
                throw new ArgumentNullException(nameof(queryParams));

            if (linkGenerator is null)
                throw new ArgumentNullException(nameof(linkGenerator));

            return new PaginationHeader(
                pagedCollection.CurrentPageNumber,
                pagedCollection.PageSize,
                pagedCollection.PageCount,
                pagedCollection.ItemCount,
                GetModifiedUrl(linkGenerator.GetUriByRouteValues(httpContext, routeName, new { page = pagedCollection.NextPageNumber, limit = pagedCollection.PageSize }), includeOnlyQueryString),
                GetModifiedUrl(linkGenerator.GetUriByRouteValues(httpContext, routeName, new { page = pagedCollection.PreviousPageNumber, limit = pagedCollection.PageSize }), includeOnlyQueryString),
                GetModifiedUrl(linkGenerator.GetUriByRouteValues(httpContext, routeName, new { page = 1, limit = pagedCollection.PageSize }), includeOnlyQueryString),
                GetModifiedUrl(linkGenerator.GetUriByRouteValues(httpContext, routeName, new { page = pagedCollection.LastPageNumber, limit = pagedCollection.PageSize }), includeOnlyQueryString),
                GetModifiedUrl(linkGenerator.GetUriByRouteValues(httpContext, routeName, new { page = pagedCollection.CurrentPageNumber, limit = pagedCollection.PageSize }), includeOnlyQueryString)
            );
        }

        private static string? GetModifiedUrl(string url, bool includeOnlyQueryString)
        {
            if (string.IsNullOrWhiteSpace(url))
                return null;

            return includeOnlyQueryString ? GetQueryString(url) : url;
        }

        private static string GetQueryString(string url) => url.Split('?')[1];
    }
}
