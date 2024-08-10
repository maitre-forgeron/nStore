using CartingService.BLL.Shared;
using NStore.Shared.Exceptions;
using NStore.Shared.Extensions;

namespace CartingService.BLL.Carting;

public class Cart : AggregateRoot
{
    public string CartId { get; private set; }

    public List<int> Items { get; private set; } = new();

    private Cart()
    {
    }

    public Cart(string cartId, List<int> items) : base()
    {
        CartId = cartId;
        Items = items;

        Validate(this);
    }

    public void AddItem(int itemId)
    {
        if (Items.Any(x => x == itemId))
        {
            throw new DomainException(DomainErrorMessagesProvider.AlreadyExistsInObjectErrorMessage(nameof(Item), "Id", itemId.ToString(), nameof(Cart)), ExceptionLevel.Warning);
        }

        Items.Add(itemId);

        base.Update();
    }

    public void RemoveItem(int itemId)
    {
        var item = Items.Where(x => x == itemId).Select(x => new { Id = x }).FirstOrDefault();

        if (item != null)
        {
            Items.Remove(item.Id);

            base.Update();
        }
    }

    private static void Validate(Cart cart)
    {
        if (string.IsNullOrWhiteSpace(cart.CartId))
        {
            throw new DomainException(DomainErrorMessagesProvider.ProperyRequiredErrorMessage(nameof(Cart), nameof(cart.CartId)), ExceptionLevel.Warning);
        }

        if (!cart.CartId.IsUuid())
        {
            throw new DomainException(DomainErrorMessagesProvider.InvalidUUIDErrorMessage(), ExceptionLevel.Warning);
        }
    }
}
