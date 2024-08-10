using NStore.Shared.Exceptions;
using NStore.Shared.ValueObjects;

namespace CatalogService.Domain.Entities
{
    public class Category : Entity
    {
        public string Name { get; private set; }

        public Image? Image { get; private set; }

        public int? ParentId { get; private set; }

        public Category? Parent { get; private set; }

        private List<Category> _children = new();
        public IReadOnlyCollection<Category> Children => _children.AsReadOnly();

        private List<Product> _products = new();
        public IReadOnlyCollection<Product> Products => _products.AsReadOnly();

        private Category()
        {
            
        }

        public Category(string name, Image? image, int? parentId, Category? parent)
        {
            Name = name;
            Image = image;
            ParentId = parentId;
            Parent = parent;

            Validate(this);
        }

        public void Update(string name, Image? image, int? parentId)
        {
            Name = name;
            Image = image;
            ParentId = !parentId.HasValue || parentId == 0 ? null : parentId;

            Validate(this);
        }

        public void SetParent(Category parent)
        {
            if (parent == null)
            {
                Parent = null;
                ParentId = null;

                return;
            }

            Parent = parent;
            ParentId = parent.Id;
        }

        private static void Validate(Category category)
        {
            if (string.IsNullOrWhiteSpace(category.Name))
            {
                throw new DomainException(DomainErrorMessagesProvider.ProperyRequiredErrorMessage(nameof(Category), nameof(category.Name)), ExceptionLevel.Warning);
            }
            
            if(category.Name.Length > Constants.TextMaxLength)
            {
                throw new DomainException(DomainErrorMessagesProvider.TextIsLongerThanMaxLengthErrorMessage(nameof(Category), nameof(category.Name), Constants.TextMaxLength), ExceptionLevel.Warning);
            }
        }
    }
}
