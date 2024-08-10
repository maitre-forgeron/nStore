using AutoMapper;
using CartingService.Application.Dtos;
using CartingService.BLL.Carting;
using MediatR;
using NStore.Shared.ValueObjects;

namespace CartingService.Application.Carts.Commands;

public record UpdateCartItemCommand(UpdateItemDto Item) : IRequest;

public class UpdateCartItemCommandHandler : IRequestHandler<UpdateCartItemCommand>
{
    private readonly IMapper _mapper;
    private readonly ICartService _cartService;

    public UpdateCartItemCommandHandler(ICartService cartService, IMapper mapper)
    {
        _cartService = cartService;
        _mapper = mapper;
    }

    public async Task Handle(UpdateCartItemCommand request, CancellationToken cancellationToken)
    {
        var item = await _cartService.GetItemAsync(request.Item.Id);

        if (item == null)
            return;
        
        item.Update(request.Item.Name, _mapper.Map<Image>(request.Item.Image), _mapper.Map<Money>(request.Item.Price));

        await _cartService.UpdateItemAsync(item);
    }
}