using CatalogService.Application.Dtos;
using CatalogService.Domain.Entities;
using NStore.Shared.ValueObjects;

namespace CatalogService.Application.Mappers
{
    public interface IProductsMapper : 
        IMapper<AddProductDto, Product>,
        IMapper<Product, ProductDto>,
        IMapper<MoneyDto,Money>,
        IMapper<ImageDto,Image?>,
        IMapper<Image, ImageDto?>,
        IMapper<Money, MoneyDto>,
        IMapper<Category, CategoryDto>,
        IMapper<AddCategoryDto, Category>
    {
    }
}
