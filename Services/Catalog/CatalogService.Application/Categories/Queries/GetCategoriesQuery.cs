using CatalogService.Application.Dtos;
using CatalogService.Application.Mappers;
using CatalogService.Domain.Interfaces.Repositories;
using MediatR;

namespace CatalogService.Application.Categories.Queries
{
    public record GetCategoriesQuery : IRequest<IEnumerable<CategoryDto>>;

    public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, IEnumerable<CategoryDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IProductsMapper _mapper;

        public GetCategoriesQueryHandler(IUnitOfWork uow, IProductsMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await _uow.CategoryRepository.GetAllAsync();

            var dtos = categories.Select(c => _mapper.Map(c));

            return dtos;
        }
    }
}
