using NStore.Shared;

namespace CartingService.Application.Dtos
{
    public record CartDto(string Oid, List<ItemDto> Items);

    public record ItemDto(int Id, string Name, ImageDto? Image, MoneyDto Price, int Quantity);

    public record UpdateItemDto(int Id, string Name, ImageDto? Image, MoneyDto Price);

    public record ImageDto(string Url, string AltText);

    public record MoneyDto(decimal Amount, Currency Currency);

    public record AddItemRequest(string Oid, ItemDto Item);

    public record AddItemResponse(ItemDto Item);

    public record GetItemsResponse(string Oid, List<ItemDto> Items);
}
