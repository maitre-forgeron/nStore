using CatalogService.Application.Dtos;
using CatalogService.Domain.Entities;
using NStore.Shared.ValueObjects;

namespace CatalogService.Application.Mappers
{
    public class ProductsMapper : IProductsMapper
    {
        public Product Map(AddProductDto dto) => new (dto.Name, dto.Description, Map(dto.Image), Map(dto.Price), dto.Amount, dto.CategoryId);

        public ProductDto Map(Product src) => new (src.Id, src.Name, src.Description, Map(src.Image), Map(src.Price), src.Amount, Map(src.Category));

        public Money Map(MoneyDto dto) => new (dto.Amount, dto.Currency);
        
        public Image? Map(ImageDto dto) => dto == null ? null : new (dto.Url, dto.AltText);
        
        public ImageDto? Map(Image image) => image == null ? null : new (image.Url, image.AltText);

        public MoneyDto Map(Money money) => new(money.Amount, money.Currency);

        public CategoryDto Map(Category category) => new (category.Id, category.Name, category.ParentId);

        public Category Map(AddCategoryDto src) => new(src.Name, Map(src.Image), !src.ParentId.HasValue || src.ParentId == 0 ? null : src.ParentId, null);
    }
}
