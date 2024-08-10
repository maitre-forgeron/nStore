namespace CatalogService.Application.Mappers
{
    public interface IMapper<TSrc, TDest>
    {
        TDest Map(TSrc src);
    }
}
