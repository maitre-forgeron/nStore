namespace CatalogService.Domain.Entities
{
    public class AggregateRoot : Entity
    {
        protected AggregateRoot()
        {

        }

        protected AggregateRoot(int id) : base(id)
        {

        }
    }
}