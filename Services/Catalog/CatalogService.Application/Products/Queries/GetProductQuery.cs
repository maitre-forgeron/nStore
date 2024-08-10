using CatalogService.Application.Dtos;
using CatalogService.Application.Mappers;
using CatalogService.Domain.Interfaces.Repositories;
using MediatR;

namespace CatalogService.Application.Products.Queries
{
    public record GetProductQuery(int id) : IRequest<ProductDto>;


    public class GetProductQueryHandler : IRequestHandler<GetProductQuery, ProductDto>
    {
        private readonly IUnitOfWork _uow;
        private readonly IProductsMapper _mapper;

        public GetProductQueryHandler(IUnitOfWork uow, IProductsMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<ProductDto> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var product = await _uow.ProductRepository.GetByIdAsync(request.id, p => p.Category);

            if (product == null)
            {
                return null;
            }

            var dto = _mapper.Map(product);

            return dto;
        }
    }
}
