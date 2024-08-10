using NStore.Shared.Exceptions;
using NStore.Shared.ValueObjects;

namespace CatalogService.Domain.Entities
{
    public class Product : AggregateRoot
    {
        public string Name { get; private set; }

        public string Description { get; private set; }

        public Image? Image { get; private set; }

        public Money Price { get; private set; }

        public int Amount { get; private set; }

        public int CategoryId { get; private set; }

        public Category Category { get; private set; }

        private Product() 
        {
        }

        public Product(string name, string description, Image image, Money price, int amount, int categoryId)
        {
            Name = name;
            Description = description;
            Image = image;
            Price = price;
            Amount = amount;
            CategoryId = categoryId;

            CreateDate = DateTime.UtcNow;
        }

        public void Update(string name, string description, Image image, Money price, int amount, int categoryId)
        {
            Name = name;
            Description = description;
            Image = image;
            Price = price;
            Amount = amount;
            CategoryId = categoryId;

            UpdateDate = DateTime.UtcNow;

            Validate(this);
        }

        private static void Validate(Product product)
        {
            if (string.IsNullOrWhiteSpace(product.Name))
            {
                throw new DomainException(DomainErrorMessagesProvider.ProperyRequiredErrorMessage(nameof(Product), nameof(product.Name)), ExceptionLevel.Warning);
            }

            if (product.Name.Length > Constants.TextMaxLength)
            {
                throw new DomainException(DomainErrorMessagesProvider.TextIsLongerThanMaxLengthErrorMessage(nameof(Product), nameof(product.Name), Constants.TextMaxLength), ExceptionLevel.Warning);
            }

            if (product.CategoryId <= 0)
            {
                throw new DomainException(DomainErrorMessagesProvider.ProperyRequiredErrorMessage(nameof(Product), nameof(product.CategoryId)), ExceptionLevel.Warning);
            }

            if (product.Price == null)
            {
                throw new DomainException(DomainErrorMessagesProvider.ProperyRequiredErrorMessage(nameof(Product), nameof(product.Price)), ExceptionLevel.Warning);
            }

            if (product.Amount <= 0)
            {
                throw new DomainException(DomainErrorMessagesProvider.LessOrEqualToErrorMessage(nameof(Product), nameof(product.Amount), "0"), ExceptionLevel.Warning);
            }
        }
    }
}
