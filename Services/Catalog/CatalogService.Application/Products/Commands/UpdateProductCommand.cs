using CatalogService.Application.Dtos;
using CatalogService.Application.Mappers;
using CatalogService.Domain.Interfaces.Repositories;
using MassTransit;
using MediatR;
using NStore.Messages.Events;

namespace CatalogService.Application.Products.Commands
{
    public record UpdateProductCommand(UpdateProductDto Dto) : IRequest<int>;

    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, int>
    {
        private readonly IUnitOfWork _uow;
        private readonly IProductsMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;

        public UpdateProductCommandHandler(IUnitOfWork uow, IProductsMapper mapper, IPublishEndpoint publishEndpoint)
        {
            _uow = uow;
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<int> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;

            var product = await _uow.ProductRepository.GetByIdAsync(dto.Id, x => x.Category);

            if (product == null)
            {
                return 0;
            }

            var image = _mapper.Map(dto.Image);
            var price = _mapper.Map(dto.Price);

            product.Update(dto.Name, dto.Description, image, price, dto.Amount, dto.CategoryId);

            await _uow.CommitAsync();

            await _publishEndpoint.Publish(new ProductUpdated(product.Id, product.Name, product.Image, product.Price), cancellationToken);

            return product.Id;
        }
    }
}
