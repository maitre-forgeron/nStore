using CatalogService.Domain.Interfaces.Repositories;
using MediatR;
using NStore.Shared.Constants;

namespace CatalogService.Application.Categories.Commands
{
    public record DeleteCategoryCommand(int Id) : IRequest;

    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand>
    {
        private readonly IUnitOfWork _uow;

        public DeleteCategoryCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _uow.CategoryRepository.GetByIdAsync(request.Id, c => c.Products, c => c.Children);

            if (category.Children?.Any() ?? false)
            {
                throw new InvalidOperationException(ErrorMessageConstants.InvalidDeleteOperationErrorMessage);
            }

            _uow.ProductRepository.DeleteRange(category.Products);
            _uow.CategoryRepository.Delete(category);

            await _uow.CommitAsync();
        }
    }
}
