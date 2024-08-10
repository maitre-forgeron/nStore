using CatalogService.Application.Dtos;
using CatalogService.Application.Mappers;
using CatalogService.Domain.Interfaces.Repositories;
using MediatR;

namespace CatalogService.Application.Categories.Commands
{
    public record UpdateCategoryCommand(UpdateCategoryDto Dto) : IRequest<int>;

    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, int>
    {
        private readonly IUnitOfWork _uow;
        private readonly IProductsMapper _mapper;

        public UpdateCategoryCommandHandler(IUnitOfWork uow, IProductsMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<int> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _uow.CategoryRepository.GetByIdAsync(request.Dto.Id);

            if (category == null)
            {
                return 0;
            }

            category.Update(request.Dto.Name, _mapper.Map(request.Dto.Image), request.Dto.ParentId);

            await _uow.CommitAsync();

            return category.Id;
        }
    }
}
