using AutoMapper;
using CartingService.Application.Dtos;
using CartingService.BLL.Carting;
using MediatR;

namespace CartingService.Application.Carts.Commands;

public record AddItemToCartCommand(AddItemRequest Request) : IRequest<AddItemResponse>;

public class AddItemToCartCommandHandler : IRequestHandler<AddItemToCartCommand, AddItemResponse>
{
    private readonly IMapper _mapper;
    private readonly ICartService _cartService;

    public AddItemToCartCommandHandler(ICartService cartService, IMapper mapper)
    {
        _cartService = cartService;
        _mapper = mapper;
    }

    public async Task<AddItemResponse> Handle(AddItemToCartCommand command, CancellationToken cancellationToken)
    {
        var item = _mapper.Map<Item>(command.Request.Item);

        await _cartService.AddItemAsync(command.Request.Oid, item);

        var response = new AddItemResponse(command.Request.Item);

        return response;
    }
}
