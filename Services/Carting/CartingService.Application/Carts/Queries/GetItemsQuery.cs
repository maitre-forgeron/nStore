using AutoMapper;
using CartingService.Application.Dtos;
using CartingService.BLL.Carting;
using MediatR;

namespace CartingService.Application.Carts.Queries
{
    public record GetItemsQuery(string Oid) : IRequest<GetItemsResponse>;

    public class GetItemsQueryHandler : IRequestHandler<GetItemsQuery, GetItemsResponse>
    {
        private readonly IMapper _mapper;
        private readonly ICartService _cartService;

        public GetItemsQueryHandler(ICartService cartService, IMapper mapper)
        {
            _cartService = cartService;
            _mapper = mapper;
        }

        public async Task<GetItemsResponse> Handle(GetItemsQuery command, CancellationToken cancellationToken)
        {
            var cartAndItems = await _cartService.GetCartAsync(command.Oid);

            if (cartAndItems.Cart == null)
            {
                return new GetItemsResponse(null, new());
            }

            return new GetItemsResponse(command.Oid, _mapper.Map<List<ItemDto>>(cartAndItems.Items));
        }
    }
}
