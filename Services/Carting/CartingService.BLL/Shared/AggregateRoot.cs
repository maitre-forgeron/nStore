namespace CartingService.BLL.Shared;

public abstract class AggregateRoot
{
    public DateTime CreateDate { get; private set; }
    public DateTime? UpdateDate { get; private set; }

    public AggregateRoot()
    {
        CreateDate = DateTime.UtcNow;
    }

    protected virtual void Update()
    {
        UpdateDate = DateTime.UtcNow;
    }
}
