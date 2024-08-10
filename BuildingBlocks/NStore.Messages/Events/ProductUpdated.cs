using NStore.Shared.ValueObjects;

namespace NStore.Messages.Events;

public record ProductUpdated
{
    public int ProductId { get; init; }

    public string Name { get; init; }

    public Image? Image { get; init; }

    public Money Price { get; init; }

    public DateTime CreationDate { get; init; }

    public ProductUpdated(int productId, string name, Image? image, Money price)
        : base()
    {
        ProductId = productId;
        Name = name;
        Image = image;
        Price = price;

        CreationDate = DateTime.UtcNow;
    }
}