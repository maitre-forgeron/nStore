using CatalogService.Application.Dtos;
using CatalogService.Application.Mappers;
using CatalogService.Domain.Interfaces.Repositories;
using MediatR;

namespace CatalogService.Application.Categories.Commands
{
    public record AddCategoryCommand(AddCategoryDto Dto) : IRequest<int>;

    public class AddCategoryCommandHandler : IRequestHandler<AddCategoryCommand, int>
    {
        private readonly IUnitOfWork _uow;
        private readonly IProductsMapper _mapper;

        public AddCategoryCommandHandler(IUnitOfWork uow, IProductsMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<int> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
        {
            var parentCategory = request.Dto.ParentId.HasValue ? await _uow.CategoryRepository.GetByIdAsync(request.Dto.ParentId.Value) : null;

            var category = _mapper.Map(request.Dto);

            category.SetParent(parentCategory);

            _uow.CategoryRepository.Add(category);

            await _uow.CommitAsync();

            return category.Id;
        }
    }
}
