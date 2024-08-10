using CatalogService.Application.Dtos;
using CatalogService.Application.Mappers;
using CatalogService.Domain.Interfaces.Repositories;
using MediatR;

namespace CatalogService.Application.Products.Commands
{
    public record AddProductCommand(AddProductDto Dto) : IRequest<ProductDto>;

    public class AddProductCommandHandler : IRequestHandler<AddProductCommand, ProductDto>
    {
        private readonly IUnitOfWork _uow;
        private readonly IProductsMapper _mapper;

        public AddProductCommandHandler(IUnitOfWork uow, IProductsMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<ProductDto> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            var product = _mapper.Map(request.Dto);

            _uow.ProductRepository.Add(product);

            await _uow.CommitAsync();

            var savedProduct = await _uow.ProductRepository.GetByIdAsync(product.Id, p => p.Category);
            var dto = _mapper.Map(savedProduct);

            return dto;
        }
    }
}
