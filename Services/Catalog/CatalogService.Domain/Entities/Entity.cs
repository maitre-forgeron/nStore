using NStore.Shared.Exceptions;

namespace CatalogService.Domain.Entities
{
    public abstract class Entity
    {
        public int Id { get; protected set; }

        public DateTime CreateDate { get; protected set; }

        public DateTime? UpdateDate { get; protected set; }

        protected Entity()
        {
        }

        protected Entity(int id)
        {
            if (id <= 0)
            {
                throw new DomainException("Id is required", ExceptionLevel.Fatal);
            }

            CreateDate = DateTime.UtcNow;
        }

        protected virtual void Update()
        {
            UpdateDate = DateTime.UtcNow;
        }
    }
}
