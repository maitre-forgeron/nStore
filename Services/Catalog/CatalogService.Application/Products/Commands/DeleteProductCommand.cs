using CatalogService.Domain.Interfaces.Repositories;
using MediatR;

namespace CatalogService.Application.Products.Commands
{
    public record DeleteProductCommand(int Id) : IRequest;

    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
    {
        private readonly IUnitOfWork _uow;

        public DeleteProductCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            await _uow.ProductRepository.DeleteAsync(request.Id);

            await _uow.CommitAsync();
        }
    }
}
