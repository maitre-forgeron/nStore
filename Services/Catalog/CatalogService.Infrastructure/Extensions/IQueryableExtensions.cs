﻿using Microsoft.EntityFrameworkCore;
using NStore.Shared.Collections;

namespace CatalogService.Infrastructure.Extensions
{
    public static class IQueryableExtensions
    {
        public static Task<IPagedCollection<T>> ToPagedCollectionAsync<T>(
            this IQueryable<T> source,
            int pageNumber,
            int pageSize)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source), "Value may not be null");

            if (pageNumber < 0)
                throw new ArgumentOutOfRangeException(nameof(pageNumber), "Value must be greater than or equal to zero");

            if (pageSize < 0)
                throw new ArgumentOutOfRangeException(nameof(pageSize), "Value must be greater than or equal to zero");

            async Task<IPagedCollection<T>> ToPagedCollectionAsync()
            {
                var itemCount = await source.CountAsync().ConfigureAwait(false);

                var items = await source
                    .Skip(pageSize * (pageNumber - 1))
                    .Take(pageSize)
                    .ToListAsync()
                    .ConfigureAwait(false);

                return new PagedCollection<T>(items, itemCount, pageNumber, pageSize);
            }

            return ToPagedCollectionAsync();
        }
    }
}
