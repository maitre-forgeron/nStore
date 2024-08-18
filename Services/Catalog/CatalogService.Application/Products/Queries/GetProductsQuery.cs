using CatalogService.Application.Dtos;
using CatalogService.Application.Mappers;
using CatalogService.Domain.Entities;
using CatalogService.Domain.Interfaces.Repositories;
using CatalogService.Infrastructure.Extensions;
using MediatR;
using NStore.Shared.Collections;

namespace CatalogService.Application.Products.Queries
{
    public record GetProductsQuery(GetProductsDto Dto) : IRequest<IPagedCollection<ProductDto>>;

    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, IPagedCollection<ProductDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IProductsMapper _mapper;

        public GetProductsQueryHandler(IUnitOfWork uow, IProductsMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<IPagedCollection<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var productsQuery = GetQueryWithFilters(request).OrderBy(x => x.CreateDate);

            var products = await productsQuery.ToPagedCollectionAsync(request.Dto.Page, request.Dto.Limit);

            var dtos = products.Select(_mapper.Map).ToList();

            return new PagedCollection<ProductDto>(dtos, products.ItemCount, products.CurrentPageNumber, products.PageSize);
        }

        private IQueryable<Product> GetQueryWithFilters(GetProductsQuery request)
        {
            return _uow.ProductRepository.GetQueryable(null, false, p => p.Category);
        }
    }
}
