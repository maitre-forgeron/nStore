using CatalogService.Application.Dtos;
using CatalogService.Application.Mappers;
using CatalogService.Domain.Interfaces.Repositories;
using MediatR;

namespace CatalogService.Application.Categories.Queries;

public record GetCategoryQuery(int Id) : IRequest<CategoryDto>;

public class GetCategoryQueryHandler : IRequestHandler<GetCategoryQuery, CategoryDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IProductsMapper _mapper;

    public GetCategoryQueryHandler(IUnitOfWork uow, IProductsMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<CategoryDto> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
    {
        var category = await _uow.CategoryRepository.GetByIdAsync(request.Id);

        var dto = _mapper.Map(category);

        return dto;
    }
}