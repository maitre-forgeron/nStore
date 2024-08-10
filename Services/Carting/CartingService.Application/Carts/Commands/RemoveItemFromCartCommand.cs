using CartingService.BLL.Carting;
using MediatR;

namespace CartingService.Application.Carts.Commands;

public record RemoveItemFromCartCommand(string Oid, int ItemId) : IRequest;

public class RemoveItemFromCartCommandHandler : IRequestHandler<RemoveItemFromCartCommand>
{
    private readonly ICartService _cartService;

    public RemoveItemFromCartCommandHandler(ICartService cartService)
    {
        _cartService = cartService;
    }

    public async Task Handle(RemoveItemFromCartCommand command, CancellationToken cancellationToken)
    {
        await _cartService.RemoveItemAsync(command.Oid, command.ItemId);
    }
}
