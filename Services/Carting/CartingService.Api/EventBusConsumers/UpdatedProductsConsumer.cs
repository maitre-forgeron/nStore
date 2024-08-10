using AutoMapper;
using CartingService.Application.Carts.Commands;
using CartingService.Application.Dtos;
using MassTransit;
using MediatR;
using NStore.Messages.Events;

namespace CartingService.Api.EventBusConsumers;

public class UpdatedProductsConsumer : IConsumer<ProductUpdated>
{
    private readonly IMapper _mapper;
    private readonly ISender _mediator;

    public UpdatedProductsConsumer(IMapper mapper, ISender mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<ProductUpdated> context)
    {
        var itemDto = new UpdateItemDto(context.Message.ProductId, context.Message.Name, _mapper.Map<ImageDto>(context.Message.Image), _mapper.Map<MoneyDto>(context.Message.Price));

        await _mediator.Send(new UpdateCartItemCommand(itemDto));
    }
}