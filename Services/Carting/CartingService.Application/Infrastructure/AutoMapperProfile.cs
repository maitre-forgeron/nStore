using AutoMapper;
using CartingService.Application.Dtos;
using CartingService.BLL.Carting;
using NStore.Shared.ValueObjects;

namespace CartingService.Application.Infrastructure.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Cart, CartDto>();
            CreateMap<Item, ItemDto>();

            CreateMap<Money, MoneyDto>();
            CreateMap<Image, ImageDto>();
            CreateMap<MoneyDto, Money>()
                .ConstructUsing(src => new Money(src.Amount, src.Currency));
            CreateMap<ImageDto, Image>()
                .ConstructUsing(src => new Image(src.Url, src.AltText));

            CreateMap<ItemDto, Item>()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => new Image(src.Image.Url, src.Image.AltText)))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => new Money(src.Price.Amount, src.Price.Currency)));

            CreateMap<CartDto, Cart>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(x => x.Items));
        }
    }
}
