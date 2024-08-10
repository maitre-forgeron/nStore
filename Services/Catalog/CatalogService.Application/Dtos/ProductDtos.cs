using NStore.Shared;

namespace CatalogService.Application.Dtos
{
    public record ImageDto(string Url, string AltText);

    public record MoneyDto(decimal Amount, Currency Currency);

    public record CategoryDto(int Id, string Name, int? ParentId);

    public record AddCategoryDto(string Name, ImageDto? Image, int? ParentId);

    public record UpdateCategoryDto(int Id, string Name, ImageDto? Image, int? ParentId);

    public record GetProductsDto(int Page, int Limit, int? CategoryId);

    public record ProductDto(int Id, string Name, string Description, ImageDto? Image, MoneyDto Price, int Amount, CategoryDto Category);

    public record AddProductDto(string Name, string Description, ImageDto? Image, MoneyDto Price, int Amount, int CategoryId);

    public record UpdateProductDto(int Id, string Name, string Description, ImageDto? Image, MoneyDto Price, int Amount, int CategoryId) : AddProductDto(Name, Description, Image, Price, Amount, CategoryId);
}
